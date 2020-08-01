using System.Collections;
using System.Collections.Generic;

public interface IItemFactory
{
    void AppendItems(ref Dictionary<string, Item> items);
}

public abstract class Item
{
    private ItemComponent itemComponent;
    public ItemComponent Component
    {
        get
        {
            return itemComponent;
        }
        set
        {
            itemComponent = value;
            UpdateItemComponent();
        }
    }
    public readonly string Id;
    public string Name;
    public Item(string id)
    {
        Id = id;
    }
    protected abstract void UpdateItemComponent();
}
