using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PurchaseCategoryEnum
{
    seed,
    sprinkler
}

public static class PurchaseCategoryTranslate
{
    public static string Translate(this PurchaseCategoryEnum category)
    {
        if (GameManager.Instance.language == Language.ko_KR)
        {
            switch (category)
            {
                case PurchaseCategoryEnum.seed:
                    return "씨앗";
                case PurchaseCategoryEnum.sprinkler:
                    return "물뿌리개";
            }
        }
        return "Invalid";
    }
}

public class PurchaseManager : MonoBehaviour
{
    public GameObject PurchaseCategoryContent;
    public GameObject PurchaseCategoryPrefab;
    public GameObject PurchaseButtonPrefab;
    private Dictionary<PurchaseCategoryEnum, PurchaseCategory> categories;
    private List<IPurchaseAble> purchaseAbles; 
    private static PurchaseManager _instance;
    public static PurchaseManager Instance
    {
        get {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(PurchaseManager)) as PurchaseManager;
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
        categories = new Dictionary<PurchaseCategoryEnum, PurchaseCategory>();
        purchaseAbles = new List<IPurchaseAble>();
        CreateAllCategory();
    }

    public void CreateAllCategory()
    {
        foreach (PurchaseCategoryEnum name in Enum.GetValues(typeof(PurchaseCategoryEnum)))
        {
            GameObject category = Instantiate(PurchaseCategoryPrefab, Vector3.zero, Quaternion.identity);
            PurchaseCategory purchaseCategory = category.GetComponent<PurchaseCategory>();
            purchaseCategory.CategoryName = name.Translate();
            categories.Add(name, purchaseCategory);
            category.transform.SetParent(PurchaseCategoryContent.transform);
        }
    }

    public void AddPurchaseButton<T>(T purchaseAble) where T : Item, IPurchaseAble 
    {
        GameObject obj = Instantiate(PurchaseButtonPrefab, Vector3.zero, Quaternion.identity);
        PurchaseButton purchaseButton = obj.GetComponent<PurchaseButton>();
        purchaseButton.GoodsPrice = purchaseAble.PurchasePrice;
        purchaseButton.Goods = purchaseAble;
        purchaseButton.GoodsItem = purchaseAble;
        purchaseAble.PurchaseButtonObject = purchaseButton;
        purchaseAble.UpdatePurchaseButton();
        purchaseAbles.Add(purchaseAble);
        categories[purchaseAble.Category].AddPurchaseButton(purchaseButton);
        Debug.Log("PurchaseButton Added");
    }

    // public bool GetItem(Item item, int count = 1)
    // {
    //     // TODO: 실패하면 false 반환
    //     if (inventory.ContainsKey(item.Id))
    //     {
    //         inventory[item.Id].Component.ItemCount += count;
    //         return true;
    //     }
    //     GameObject obj = Instantiate(ItemElementPrefab, Vector3.zero, Quaternion.identity);
    //     obj.gameObject.transform.SetParent(ItemContent.transform, true);
    //     obj.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    //     item.Component = obj.GetComponent<ItemComponent>();
    //     item.Component.SetItem(item);
    //     item.Component.ItemCount = count;
    //     inventory.Add(item.Id, item);
    //     return true;
    // }
}
