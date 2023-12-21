using Assets.Scripts.EventSystem;
using Assets.Scripts.Helpers;
using Assets.Scripts.Helpers.ListView;
using Assets.Scripts.Registries;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListViewController : MonoBehaviour 
{
    private List<object> data;
    private object selectedData;
    private IListItem selectedItem;
    private List<IListItem> items;

    public Action selectionChanged;

    public GameObject prefab;
    public GameObject contentParent;
    public float childScale = 1;

    public bool allowSelection = true;

    public object Selected 
    {
        get => selectedData;
        set
        {
            selectedData = value;
            selectedItem = items.Find(i => i.HasData(value));
            selectionChanged?.Invoke();
        } 
    }

    public void InitView(List<object> data,
        object selection = null)
    {
        this.data = data;
        items = new List<IListItem>();
        RefreshList();

        Selected = selection;

        if (selection != null) 
            ChangeSelection(Selected);
    }

    public void InitView(IListViewExtendedRegistry reg,
        object selection = null)
    {
        data = new();
        reg.ForEach(i => data.Add(i));
        items = new List<IListItem>();
        RefreshList();

        Selected = selection;

        if (selection != null)
            ChangeSelection(Selected);
    }

    public void RefreshList(List<object> newData = null)
    {
        items.Clear();

        foreach (Transform child in contentParent.transform)
            Destroy(child.gameObject);

        if (newData != null) 
            data = newData;
        
        data.ForEach(item =>
        {
            var obj = Instantiate(prefab, contentParent.transform);
            var comp = obj.GetComponent<IListItem>();

            obj.transform.localScale *= childScale;

            if (allowSelection)
                comp.Selection += () => ChangeSelection(item);

            comp.FillItem(item);
            items.Add(comp);
        });
            
    }

    public void ChangeSelection(object obj)
    {
        Selected = data.Find(i => i == obj);

        selectedItem?.OnSelected();
    }
}
