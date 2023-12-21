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
    public List<EquipmentItem> CollectedItems { get; private set; }


    public ResourceDescriptions ResourceRegistry;
    public EquipmentItemRegistry EquipmentItemRegistry;

    [Header("Resource collecting messages")]
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
    }

    public void CollectItem(EquipmentItem item)
    {
        CollectedItems.Add(item);
    }

    private void Start()
    {
        if (!EquipmentItemRegistry.GetAll().Any())
        {
            throw new Exception("Empty item register");
        }

        CollectedResources = new List<CollectedResourceData>();
        CollectedItems = new List<EquipmentItem>();
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
    }
}
