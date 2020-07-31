using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedFactory : MonoBehaviour, IItemFactory
{
    public void AppendItems(ref Dictionary<string, Item> items)
    {
        FlowerFactory factory = GameObject.Find("FlowerFactory").GetComponent<FlowerFactory>();
        // 반드시 FlowerFactory::AppendItems 실행이 완료된 후 호출되야 함
        if (!factory.IsInitialized)
        {
            Debug.LogError("FlowerFactory 먼저");
        }
        foreach (Flower flower in factory.flowers)
        {
            Seed seed = new Seed(flower);
            items.Add(seed.Id, seed);
        }
    }
}

public class Seed : Item
{
    private Flower flower;
    public Seed(Flower flower) : base(flower.Id + "_seed")
    {
        this.flower = flower;
    }
    protected override void UpdateItemComponent()
    {
        Component.ItemName = flower.Name;
        Component.ItemTexture = Resources.Load("ItemPNG/seed") as Texture;
        GameObject button = InventoryManager.Instance.CreateSellButton();
        Component.AddButton(button);
    }
}