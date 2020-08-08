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
            TradeManager.Instance.AddPurchaseButton(seed);
        }
    }
}

public class Seed : Item, IPurchaseAble, ISaleAble
{
    private Flower flower;
    public Flower Flower
    {
        get => flower;
    }
    private Texture texture;
    public Seed(Flower flower) : base(flower.Id + "_seed")
    {
        this.flower = flower;
        texture = Resources.Load("ItemPNG/seed") as Texture;
    }
    protected override void UpdateItemComponent()
    {
        Component.ItemName = flower.Name;
        Component.ItemTexture = texture;
        GameObject button = InventoryManager.Instance.CreateSellButton(this);
        Component.AddButton(button);
    }

    public void UpdatePlantSeedDialogElement(ItemComponent component)
    {
        component.ItemName = flower.Name;
        component.ItemTexture = texture;
    }

    public void UpdatePurchaseButton()
    {
        PurchaseButtonObject.GoodsName = flower.Name;
        PurchaseButtonObject.GoodsTexture = texture;
        PurchaseButtonObject.GoodsPrice = PurchasePrice;
    }

    public int PurchasePrice
    {
        get
        {
            return 500;
        }
    }

    public int SalePrice
    {
        get
        {
            return PurchasePrice / 4;
        }
    }

    public PurchaseCategoryEnum Category
    {
        get
        {
            return PurchaseCategoryEnum.seed;
        }
    }

    public bool IsPurchaseAble
    {
        get
        {
            // TODO: 발견한 씨앗만 등장하게
            return true;
        }
    }

    private PurchaseButton purchaseButton;

    public PurchaseButton PurchaseButtonObject
    {
        get
        {
            return purchaseButton;
        }
        set
        {
            purchaseButton = value;
        }
    }
}