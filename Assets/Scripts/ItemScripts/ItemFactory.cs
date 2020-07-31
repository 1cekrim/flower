using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory : MonoBehaviour
{
    private Dictionary<string, Item> allItems;
    private void Start()
    {
        allItems = new Dictionary<string, Item>();
        foreach (Transform child in transform)
        {
            IItemFactory factory = child.gameObject.GetComponent<IItemFactory>();
            Debug.Log("CreateItems: " + child.name);
            CreateItems(factory);
        }
        // for debug
        foreach (Item item in allItems.Values)
        {
            InventoryManager.Instance.GetItem(item);
        }
    }
    public void CreateItems(IItemFactory factory)
    {
        factory.AppendItems(ref allItems);
    }
}
