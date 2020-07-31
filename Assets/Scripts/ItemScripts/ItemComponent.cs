using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemComponent : MonoBehaviour
{
    [SerializeField]
    private RawImage itemImage;
    [SerializeField]
    private Text itemText;
    [SerializeField]
    private Text itemCount;
    private int itemCountInt;
    [SerializeField]
    private GameObject itemCountBox;
    [SerializeField]
    private GameObject buttonGroup;
    private Item item;

    public void SetItem(Item item)
    {
        this.item = item;
    }

    public Texture ItemTexture
    {
        get
        {
            return itemImage.texture;
        }
        set
        {
            itemImage.texture = value;
        }
    }

    public string ItemName
    {
        get
        {
            return itemText.text;
        }
        set
        {
            itemText.text = value;
        }
    }

    public int ItemCount
    {
        get
        {
            return itemCountInt;
        }
        set
        {
            itemCountInt = value;
            itemCount.text = value.ToString();
            itemCountBox.SetActive(value > 1);
        }
    }

    public void AddButton(GameObject go)
    {
        go.transform.SetParent(buttonGroup.transform);
    }
}
