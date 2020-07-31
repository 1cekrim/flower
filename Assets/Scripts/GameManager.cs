using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent awakeEvents;
    public UnityEvent startEvents;
    public UnityEvent tickEvents;
    public GameObject playerObject;
    public FloorTile[,] mapTile;
    public Language language = Language.ko_KR;
    private static GameManager _instance;
    private int fixedUpdateCounter = 0;
    public static GameManager Instance
    {
        get {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;
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

        awakeEvents.Invoke();
    }
    private void Start()
    {
        startEvents.Invoke();
    }

    private void FixedUpdate()
    {
        if (++fixedUpdateCounter % 10 == 0)
        {
            tickEvents.Invoke();
        }
    }
}