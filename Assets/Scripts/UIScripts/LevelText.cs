using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelText : MonoBehaviour
{
    private Text text;
    private void Awake()
    {
        text = GetComponent<Text>();
        UpdateText();
    }
    private void Start()
    {
        UpdateText();
    }
    public void UpdateText()
    {
        if (text == null)
        {
            return;
        }
        if (ResourceManager.Instance.Level / 10 == 0)
        {
            text.text = "Lv. " + ResourceManager.Instance.Level.ToString();
        }
        else
        {
            text.text = "Lv." + ResourceManager.Instance.Level.ToString();
        }
    }
}