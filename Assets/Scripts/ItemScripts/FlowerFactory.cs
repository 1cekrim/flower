using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum FlowerColor
{
    white,
    red,
    yellow,
    purple,
    pink,
    black,
    orange,
}

public enum FlowerKind
{
    chrysanthemum,
    morning_glory,
    camellia,
    lily,
    azalea,
    rose,
    cosmos,
    tulip
}

public static class FolwerTranslate
{
    public static string Translate(this FlowerColor color)
    {
        if (GameManager.Instance.language == Language.ko_KR)
        {
            switch (color)
            {
                case FlowerColor.white:
                    return "하양";
                case FlowerColor.red:
                    return "빨강";
                case FlowerColor.yellow:
                    return "노랑";
                case FlowerColor.purple:
                    return "보라";
                case FlowerColor.pink:
                    return "분홍";
                case FlowerColor.black:
                    return "검정";
                case FlowerColor.orange:
                    return "주황";
            }
        }
        return "Invalid";
    }
    public static string Translate(this FlowerKind color)
    {
        if (GameManager.Instance.language == Language.ko_KR)
        {
            switch (color)
            {
                case FlowerKind.chrysanthemum:
                    return "국화";
                case FlowerKind.morning_glory:
                    return "나팔꽃";
                case FlowerKind.camellia:
                    return "동백꽃";
                case FlowerKind.lily:
                    return "백합";
                case FlowerKind.azalea:
                    return "진달래";
                case FlowerKind.rose:
                    return "장미";
                case FlowerKind.cosmos:
                    return "코스모스";
                case FlowerKind.tulip:
                    return "튤립";
            }
        }
        return "Invalid";
    }
}

[Serializable]
struct FlowerData
{
    public FlowerKind kind;
    public FlowerColor[] colors;
}

public class FlowerFactory : MonoBehaviour, IItemFactory
{
    public bool IsInitialized = false;
    [SerializeField]
    private GameObject bookContent;
    [SerializeField]
    private GameObject bookFlowerPrefab;
    [SerializeField]
    private FlowerData[] flowerData;
    public List<Flower> flowers = new List<Flower>();
    public void AppendItems(ref Dictionary<string, Item> items)
    {
        foreach (FlowerData data in flowerData)
        {
            GameObject obj = Instantiate(bookFlowerPrefab, Vector3.zero, Quaternion.identity);
            obj.transform.SetParent(bookContent.transform);
            obj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            BookFlower bookFlower = obj.GetComponent<BookFlower>();
            bookFlower.Name = data.kind.Translate();
            foreach (FlowerColor color in data.colors)
            {
                string id = data.kind.ToString() + "_" + color;
                Flower flower = new Flower(data.kind, color, id);
                items.Add(id, flower);
                flowers.Add(flower);
                
                // 나중에 빼기
                bookFlower.AddColor(color);
            }
        }
        IsInitialized = true;
    }
}

public class Flower : Item, ISaleAble
{
    private FlowerKind kind;
    private FlowerColor color;
    // TODO: 교배 단계 Rank 구현해야함
    public int Rank = 1;
    public Flower(FlowerKind kind, FlowerColor color, string id) : base(id)
    {
        this.kind = kind;
        this.color = color;
        Name = string.Format("{0}({1})", kind.Translate(), color.Translate());
    }
    protected override void UpdateItemComponent()
    {
        Component.ItemName = Name;
        Component.ItemTexture = Resources.Load("ItemPNG/flower") as Texture;
        GameObject button = InventoryManager.Instance.CreateSellButton(this);
        Component.AddButton(button);
    }
    public int SalePrice
    {
        get
        {
            return (int)(500 * (Rank + 1) * 1.5);
        }
    }
}