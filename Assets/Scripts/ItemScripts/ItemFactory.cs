using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory : MonoBehaviour
{
    private Dictionary<string, Item> allItems;
    public void CreateAllItems()
    {
        allItems = new Dictionary<string, Item>();
        foreach (Transform child in transform)
        {
            IItemFactory factory = child.gameObject.GetComponent<IItemFactory>();
            Debug.Log("OperateFactory: " + child.name);
            OperateFactory(factory);
        }
        // for debug
        // foreach (Item item in allItems.Values)
        // {
        //     InventoryManager.Instance.GetItem(item);
        // }
    }
    private void OperateFactory(IItemFactory factory)
    {
        factory.AppendItems(ref allItems);
    }
}
