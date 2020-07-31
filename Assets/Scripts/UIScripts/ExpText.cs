using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpText : MonoBehaviour
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
        text.text = ResourceManager.Instance.Exp.ToString() + "/" + ResourceManager.Instance.MaxExp.ToString();
    }
}
