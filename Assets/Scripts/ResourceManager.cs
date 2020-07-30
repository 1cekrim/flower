using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private int gold;
    public int Gold
    {
        get
        {
            return gold;
        }
    }
    public bool ChangeGold(int amount)
    {
        int result = gold + amount;
        if (result < 0)
        {
            return false;
        }
        gold = result;
        return true;
    }

    private static ResourceManager _instance;
    public static ResourceManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(ResourceManager)) as ResourceManager;
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
    }
}
