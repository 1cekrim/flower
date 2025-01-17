﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject ItemContent;
    public GameObject ItemElementPrefab;
    public GameObject ItemButtonPrefab;
    [SerializeField]
    private Dictionary<string, Item> inventory;
    private static InventoryManager _instance;
    public static InventoryManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(InventoryManager)) as InventoryManager;
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        inventory = new Dictionary<string, Item>();
    }

    public GameObject CreateSellButton<T>(T saleAbleItem) where T : Item, ISaleAble
    {
        GameObject button = Instantiate(ItemButtonPrefab, Vector3.zero, Quaternion.identity);
        button.transform.Find("Text").GetComponent<Text>().text = "판매";
        button.GetComponent<Button>().onClick.AddListener(() => { TradeManager.Instance.Sale(saleAbleItem); });
        return button;
    }

    public bool GetItem(Item item, int count = 1)
    {
        // TODO: 실패하면 false 반환
        if (inventory.ContainsKey(item.Id))
        {
            inventory[item.Id].Component.ItemCount += count;
            return true;
        }
        GameObject obj = Instantiate(ItemElementPrefab, Vector3.zero, Quaternion.identity);
        obj.gameObject.transform.SetParent(ItemContent.transform, true);
        obj.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        item.Component = obj.GetComponent<ItemComponent>();
        item.Component.SetItem(item);
        item.Component.ItemCount = count;
        inventory.Add(item.Id, item);
        return true;
    }

    public bool RemoveItem(Item item, int count = 1)
    {
        ItemComponent itemComponent = inventory[item.Id].Component;
        if (itemComponent.ItemCount < count)
        {
            return false;
        }
        itemComponent.ItemCount -= count;
        return true;
    }

    public List<T> SearchItem<T>() where T : Item
    {
        List<T> items = new List<T>();
        foreach (Item item in inventory.Values)
        {
            if (item is T)
            {
                items.Add(item as T);
            }
        }
        return items;
    }
}
