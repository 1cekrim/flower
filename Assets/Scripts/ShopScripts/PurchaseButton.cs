using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseButton : MonoBehaviour
{
    [SerializeField]
    private RawImage goodsImage;
    [SerializeField]
    private Text goodsNameText;
    [SerializeField]
    private Text goodsPrice;
    [SerializeField]
    private Button buttonComponent;
    // TODO: 물건을 한번에 여러 개 사는 기능
    private int goodsPriceInt = 1;
    public IPurchaseAble Goods
    {
        get; set;
    }
    public Item GoodsItem
    {
        get; set;
    }
    public Texture GoodsTexture
    {
        get
        {
            return goodsImage.texture;
        }
        set
        {
            goodsImage.texture = value;
        }
    }

    public string GoodsName
    {
        get
        {
            return goodsNameText.text;
        }
        set
        {
            goodsNameText.text = value;
        }
    }

    public int GoodsPrice
    {
        get
        {
            return goodsPriceInt;
        }
        set
        {
            goodsPriceInt = value;
            goodsPrice.text = goodsPriceInt.ToString();
        }
    }
}
