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

    private int level;
    public int Level
    {
        get
        {
            return level;
        }
    }

    private int exp;
    public int Exp
    {
        get
        {
            return exp;
        }
    }

    public int MaxExp
    {
        get
        {
            // TODO: 최대 경험치 공식은 나중에 다시 정하기
            return level * level * level;
        }
    }

    public void ChangeExp(int amount)
    {
        exp += amount;
        if (exp >= MaxExp)
        {
            exp -= MaxExp;
            ++level;
        }
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
