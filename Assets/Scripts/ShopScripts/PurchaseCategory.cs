using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseCategory : MonoBehaviour
{
    [SerializeField]
    private Text categoryText;
    [SerializeField]
    private GameObject goodsList;

    public string CategoryName
    {
        get
        {
            return categoryText.text;
        }
        set
        {
            categoryText.text = value;
        }
    }

    public void AddPurchaseButton(PurchaseButton purchaseButton)
    {
        purchaseButton.gameObject.transform.SetParent(goodsList.transform, true);
        purchaseButton.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    }
}
