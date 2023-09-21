using Assets.Scripts.EventSystem;
using Assets.Scripts.Helpers;
using Assets.Scripts.Helpers.ListView;
using Assets.Scripts.Registries;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ListViewController : MonoBehaviour 
{
    private SerializableScriptableObject selectedData;
    private IListItem selectedItem;
    private List<IListItem> items;

    public IListViewExtendedRegistry registry;
    public GameObject prefab;
    public GameObject contentParent;

    public SerializableScriptableObject Selected 
    {
        get => selectedData;
        set
        {
            selectedData = value;
            selectedItem = items.Find(i => i.HasData(value));
        } 
    }

    public void InitView(IListViewExtendedRegistry reg,
        SerializableScriptableObject selection = null)
    {
        registry = reg;
        items = new List<IListItem>();
        RefreshList();

        Selected = selection;

        if (selection != null) 
            ChangeSelection(Selected.Guid);
    }

    public void RefreshList()
    {
        items.Clear();

        foreach (Transform child in contentParent.transform)
            Destroy(child.gameObject);

        registry.ForEach(item =>
        {
            var comp = Instantiate(prefab, contentParent.transform)
                .GetComponent<IListItem>();

            comp.FillItem(item);
            items.Add(comp);
        });
            
    }

    public void ChangeSelection(string guid)
    {
        if (Selected is not null)
            selectedItem.OnUnselected();

        Selected = registry.Find(guid);

        selectedItem?.OnSelected();
    }
}
