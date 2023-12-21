using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.EquipmentSystem;
using UnityEngine;
using UnityEngine.UI;

public class LootTabHandler : MonoBehaviour
{
    [Header("Items")]
    public GameObject ItemsContainer;
    public GameObject ItemPrefab;
    public int ItemColumnLimit = 10;
    public float ItemVerticalSpacing = 20f;
    public float ItemHorisontalSpacing = 100f;
    public TextAnchor ItemHorisontalAligment = TextAnchor.UpperCenter;



    [Header("Resourses")]
    public GameObject ResourcesContainer;
    public GameObject ResoursePrefab;
    public int ResourceColumnLimit = 10;
    public float ResourceVerticalSpacing = 20f;
    public float ResourceHorisontalSpacing = 100f;
    public TextAnchor ResourceHorisontalAligment = TextAnchor.UpperCenter;

    private List<GameObject> ItemRows = new List<GameObject>();
    private int currentItemColumnIndex = 0;
    private List<GameObject> ResourcesRows = new List<GameObject>();
    private int currentResourcesColumnIndex = 0;

    void Start()
    {
        ItemsContainer.GetComponent<VerticalLayoutGroup>().spacing = ItemVerticalSpacing;
        ResourcesContainer.GetComponent<VerticalLayoutGroup>().spacing = ResourceVerticalSpacing;
    }

    public void AddItem(InventoryRecord item)
    {
        GameObject itemInstance = Instantiate(ItemPrefab, Vector3.zero, Quaternion.identity);
        itemInstance.GetComponent<CollectedItemRecordData>().ItemIcon.sprite = item.item.icon;
        itemInstance.GetComponent<CollectedItemRecordData>().ItemLabel.text = $"{item.item.name}: LVL {item.level}";

        if (currentItemColumnIndex >= ItemColumnLimit || currentItemColumnIndex == 0)
        {
            currentItemColumnIndex = currentItemColumnIndex == 0 ? currentItemColumnIndex + 1 : 0;
            GameObject row = new GameObject($"Row-{ItemRows.Count}");

            row.transform.SetParent(ItemsContainer.transform);
            row.AddComponent<HorizontalLayoutGroup>();
            row.GetComponent<HorizontalLayoutGroup>().spacing = ItemHorisontalSpacing;
            row.GetComponent<HorizontalLayoutGroup>().childAlignment = ItemHorisontalAligment;

            itemInstance.transform.SetParent(row.transform);
            row.transform.localScale = Vector3.one;
            ItemRows.Add(row);
        }
        else
        {
            itemInstance.transform.SetParent(ItemRows[^1].transform);

            currentItemColumnIndex++;
        }

        itemInstance.transform.localScale = Vector3.one;
    }

    public void AddResources(CollectedResourceTransferData resources)
    {
        GameObject directColumn = null;
        if (ResourcesRows.Any())
        {
            foreach (GameObject row in ResourcesRows)
            {
                foreach (Transform column in row.transform)
                {
                    if (column.GetComponent<CollectedResourceRecordData>().ResourceName.Equals(resources.ResourceName))
                    {
                        directColumn = column.gameObject;
                        break;
                    }
                }
                if (directColumn != null)
                {
                    break;
                }
            }
        }


        if (directColumn == null)
        {
            GameObject resourceInstance = Instantiate(ResoursePrefab, Vector3.zero, Quaternion.identity);
            resourceInstance.GetComponent<CollectedResourceRecordData>().ResourceIcon.sprite = resources.ResourceIcon;
            resourceInstance.GetComponent<CollectedResourceRecordData>().ResourceLabel.text = $"X {resources.CollectedAmount}";
            resourceInstance.GetComponent<CollectedResourceRecordData>().ResourceName = resources.ResourceName;

            if (currentResourcesColumnIndex >= ResourceColumnLimit || currentResourcesColumnIndex == 0)
            {
                currentResourcesColumnIndex = currentResourcesColumnIndex == 0 ? currentResourcesColumnIndex + 1 : 0;
                GameObject row = new GameObject($"Row-{ResourcesRows.Count}");

                row.transform.SetParent(ResourcesContainer.transform);
                row.AddComponent<HorizontalLayoutGroup>();
                row.GetComponent<HorizontalLayoutGroup>().spacing = ResourceHorisontalSpacing;
                row.GetComponent<HorizontalLayoutGroup>().childAlignment = ResourceHorisontalAligment;

                resourceInstance.transform.SetParent(row.transform);
                row.transform.localScale = Vector3.one;
                ResourcesRows.Add(row);
            }
            else
            {
                resourceInstance.transform.SetParent(ResourcesRows[^1].transform);

                currentResourcesColumnIndex++;
            }

            resourceInstance.transform.localScale = Vector3.one;
        }
        else
        {
            directColumn.GetComponent<CollectedResourceRecordData>().CollectedAmount
                += resources.CollectedAmount;
        }
    }
}
