using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldText : MonoBehaviour
{
    private Text text;
    private void Awake()
    {
        text = GetComponent<Text>();
    }
    private void Start()
    {
        UpdateText();
    }
    public void UpdateText()
    {
        text.text = ResourceManager.Instance.Gold.ToString();
    }
}
