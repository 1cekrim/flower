using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookFlower : MonoBehaviour
{
    [SerializeField]
    private Text flowerNameText;
    [SerializeField]
    private GameObject bookFlowerColors;
    [SerializeField]
    private RawImage flowerImage;
    [SerializeField]
    private GameObject flowerColorTextPrefab;
    class ColorData
    {
        public FlowerColor color;
        private bool isHided;
        public bool IsHided
        {
            get => isHided;
            set
            {
                isHided = value;
                if (isHided)
                {
                    text.text = "* ????";
                }
                else
                {
                    text.text = color.Translate();
                }
            }
        }
        public Text text;
    }
    public string Name
    {
        get
        {
            return flowerNameText.text;
        }
        set
        {
            flowerNameText.text = value;
        }
    }
    public void AddColor(FlowerColor color)
    {
        GameObject obj = Instantiate(flowerColorTextPrefab, Vector3.zero, Quaternion.identity);
        ColorData data = new ColorData();

        data.text = obj.GetComponent<Text>();
        data.color = color;
        data.IsHided = true;
        obj.transform.SetParent(bookFlowerColors.transform);
    }
}
