using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.EquipmentSystem;
using Assets.Scripts.ResourceSystem;
using Unity.VisualScripting;
using UnityEngine;

public class LootHandler : MonoBehaviour
{
    private class Pair<T, V>
    {
        public T FirstItem;
        public V SecondItem;
    }

    public List<CollectedResourceData> CollectedResources { get; private set; }
    public List<InventoryRecord> CollectedItems { get; private set; }

    public LootTabHandler LootTabHandler;

    [Header("Item collecting")]

    public EquipmentItemRegistry EquipmentItemRegistry;
    public GameObject ItemCollectingMessages;
    public GameObject ItemCollectingMessagePrefab;
    public float ItemCollectingMessageExistingTime = 5f;
    private List<Pair<GameObject, float>> _itemCollectingMessagesQueue;

    [NonSerialized]
    public List<EquipmentItem> EquipmentItemsList;

    [Header("Resource collecting")]
    public ResourceDescriptions ResourceRegistry;
    public GameObject ResourceCollectingMessages;
    public GameObject ResourceCollectingMessagePrefab;
    public float ResourceCollectingMessageExistingTime = 3f;
    public int ResourceCollectingMessagesLimit = 3;
    private List<Pair<GameObject, float>> _resourceCollectingMessagesQueue;
    private List<int> _resourceCollectingMessagesDeleteQueue;

    public static LootHandler Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<LootHandler>();

                if (_instance == null)
                {
                    throw new Exception("Loot handler does not exist");
                }
            }

            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    private static LootHandler _instance;

    private LootHandler() { }

    public void CollectResource(CollectedResourceData resourceData)
    {
        if (resourceData.LootedAmount <= 0)
        {
            return;
        }

        int index = CollectedResources.FindIndex(x => x.Name.Equals(resourceData.Name));

        if (index == -1)
        {
            CollectedResources.Add(resourceData);
        }
        else
        {
            CollectedResources[index].LootedAmount += resourceData.LootedAmount;
        }

        GameObject messageInstance =
            Instantiate(ResourceCollectingMessagePrefab, Vector3.zero, Quaternion.identity);
        messageInstance.transform.SetParent(ResourceCollectingMessages.transform);
        messageInstance.GetComponent<ResourceCollectingMessageController>().Icon.sprite =
            ResourceRegistry.GetResourceIcon(resourceData.Name);
        messageInstance.GetComponent<ResourceCollectingMessageController>().Text.text =
            $"{resourceData.Name} X {resourceData.LootedAmount}";
        messageInstance.SetActive(false);

        _resourceCollectingMessagesQueue.Add(new Pair<GameObject, float>
        {
            FirstItem = messageInstance,
            SecondItem = 0f
        });

        LootTabHandler.AddResources(new CollectedResourceTransferData
        {
            ResourceName = resourceData.Name,
            CollectedAmount = resourceData.LootedAmount,
            ResourceIcon = ResourceRegistry.GetResourceIcon(resourceData.Name)
        });
    }

    public void CollectItem(InventoryRecord item)
    {
        CollectedItems.Add(item);

        GameObject messageInstance =
            Instantiate(ItemCollectingMessagePrefab, ItemCollectingMessagePrefab.transform.position, Quaternion.identity);
        messageInstance.transform.SetParent(ItemCollectingMessages.transform);

        messageInstance.transform.localPosition = Vector3.zero;

        messageInstance.GetComponent<ItemCollectingMessageController>().Icon.sprite = item.item.icon;
        messageInstance.GetComponent<ItemCollectingMessageController>().Text.text = $"{item.item.itemName} collected";
        messageInstance.SetActive(false);

        _itemCollectingMessagesQueue.Add(new Pair<GameObject, float>
        {
            FirstItem = messageInstance,
            SecondItem = 0f
        });

        LootTabHandler.AddItem(item);
    }

    private void Start()
    {
        EquipmentItemsList = EquipmentItemRegistry.GetAll();

        if (!EquipmentItemsList.Any())
        {
            throw new Exception("Empty item register");
        }

        CollectedResources = new List<CollectedResourceData>();
        CollectedItems = new List<InventoryRecord>();
        _itemCollectingMessagesQueue = new List<Pair<GameObject, float>>();
        _resourceCollectingMessagesQueue = new List<Pair<GameObject, float>>();
        _resourceCollectingMessagesDeleteQueue = new List<int>();
    }

    private void Update()
    {
        if (_resourceCollectingMessagesQueue.Any())
        {
            for (int i = 0; i < ResourceCollectingMessagesLimit && i < _resourceCollectingMessagesQueue.Count; i++)
            {
                _resourceCollectingMessagesQueue[i].FirstItem.SetActive(true);
                _resourceCollectingMessagesQueue[i].SecondItem += Time.deltaTime;

                if (_resourceCollectingMessagesQueue[i].SecondItem >= ResourceCollectingMessageExistingTime)
                {
                    _resourceCollectingMessagesDeleteQueue.Add(i);
                }
            }

            _resourceCollectingMessagesDeleteQueue = _resourceCollectingMessagesDeleteQueue.OrderByDescending(x => x).ToList();

            foreach (int index in _resourceCollectingMessagesDeleteQueue)
            {
                Destroy(_resourceCollectingMessagesQueue[index].FirstItem);

                _resourceCollectingMessagesQueue.RemoveAt(index);
            }

            _resourceCollectingMessagesDeleteQueue.Clear();
        }

        if (_itemCollectingMessagesQueue.Any())
        {
            _itemCollectingMessagesQueue[0].FirstItem.SetActive(true);
            _itemCollectingMessagesQueue[0].SecondItem += Time.deltaTime;

            if (_itemCollectingMessagesQueue[0].SecondItem >= ItemCollectingMessageExistingTime)
            {
                Destroy(_itemCollectingMessagesQueue[0].FirstItem);

                _itemCollectingMessagesQueue.RemoveAt(0);
            }
        }
    }
}
