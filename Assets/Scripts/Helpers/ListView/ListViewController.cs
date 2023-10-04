using Assets.Scripts.EventSystem;
using Assets.Scripts.Helpers;
using Assets.Scripts.Helpers.ListView;
using Assets.Scripts.Registries;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListViewController : MonoBehaviour 
{
    private List<object> data;
    private object selectedData;
    private IListItem selectedItem;
    private List<IListItem> items;

    public GameObject prefab;
    public GameObject contentParent;

    public object Selected 
    {
        get => selectedData;
        set
        {
            selectedData = value;
            selectedItem = items.Find(i => i.HasData(value));
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
            var comp = Instantiate(prefab, contentParent.transform)
                .GetComponent<IListItem>();

            comp.FillItem(item);
            items.Add(comp);
        });
            
    }

    public void ChangeSelection(object obj)
    {
        if (Selected is not null)
            selectedItem.OnUnselected();

        Selected = data.Find(i => i.Equals(obj));

        selectedItem?.OnSelected();
    }
}
