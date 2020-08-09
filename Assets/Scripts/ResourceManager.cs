using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResourceManager : MonoBehaviour
{
    public UnityEvent changeGoldEvent;
    public UnityEvent changeExpEvent;
    public UnityEvent levelUpEvent;
    public UnityEvent changeSprinklerEvent;
    public UnityEvent changeMaxSprinklerEvent;
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
        if (amount < 0)
        {
            AudioManager.Instance.PlayAudioClip(AudioManager.Instance.LoadAudioClip("effect/PurchaseSound"), 0, 2);
        }
        else
        {
            AudioManager.Instance.PlayAudioClip(AudioManager.Instance.LoadAudioClip("effect/SaleSound"), 0.5f, 2);
        }
        if (result < 0)
        {
            return false;
        }
        gold = result;
        changeGoldEvent.Invoke();
        return true;
    }

    public void ChangeGoldNoReturn(int amount)
    {
        ChangeGold(amount);
    }

    private int level = 1;
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
        if (exp < MaxExp)
        {
            changeExpEvent.Invoke();
            return;
        }
        while (exp >= MaxExp)
        {
            exp -= MaxExp;
            changeExpEvent.Invoke();
            ++level;
            levelUpEvent.Invoke();
        }
    }

    private int sprinkler;
    public int Sprinkler
    {
        get
        {
            return sprinkler;
        }
    }
    private int maxSprinkler;
    public int MaxSprinkler
    {
        get
        {
            return maxSprinkler;
        }
        set
        {
            maxSprinkler = value;
            changeMaxSprinklerEvent.Invoke();
        }
    }

    public bool ChangeSprinkler(int amount)
    {
        int result = Sprinkler + amount;
        if (result < 0)
        {
            return false;
        }
        sprinkler = result > MaxSprinkler ? MaxSprinkler : result;
        changeSprinklerEvent.Invoke();
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
