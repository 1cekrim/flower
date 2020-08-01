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

public class TradeManager : MonoBehaviour
{
    public GameObject PurchaseCategoryContent;
    public GameObject PurchaseCategoryPrefab;
    public GameObject PurchaseButtonPrefab;
    private Dictionary<PurchaseCategoryEnum, PurchaseCategory> categories;
    private List<IPurchaseAble> purchaseAbles;
    private static TradeManager _instance;
    public static TradeManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(TradeManager)) as TradeManager;
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
        purchaseButton.GetComponent<Button>().onClick.AddListener(() => { Purchase(purchaseAble); });
        categories[purchaseAble.Category].AddPurchaseButton(purchaseButton);
        Debug.Log("PurchaseButton Added");
    }

    public void Purchase<T>(T purchaseAble, int amount = 1) where T : Item, IPurchaseAble
    {
        // TODO: 살건지 물어보는 다이얼로그 띄우기
        // 여러 개 사는거 가능하게
        if (ResourceManager.Instance.ChangeGold(-purchaseAble.PurchasePrice * amount))
        {
            InventoryManager.Instance.GetItem(purchaseAble, amount);
        }
        // TODO: 돈 부족하면 부족하다고 다이얼로그 띄우기 <- ResoureManager에서 담당하게 하는 게 좋을 듯
    }

    public void Sale<T>(T saleAble, int amount = 1) where T : Item, ISaleAble
    {
        // TODO: 팔건지 물어보는 다이얼로그 띄우기
        // 아래는 사실상 불가능한 상황
        if (saleAble.Component.ItemCount < amount)
        {
            Debug.LogError("Sale Error");
            return;
        }
        InventoryManager.Instance.RemoveItem(saleAble, amount);
        ResourceManager.Instance.ChangeGold(saleAble.SalePrice * amount);
    }
}
