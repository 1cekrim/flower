using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SprinklerText : MonoBehaviour
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
        text.text = ResourceManager.Instance.Sprinkler.ToString() + "/" + ResourceManager.Instance.MaxSprinkler.ToString();
    }
}