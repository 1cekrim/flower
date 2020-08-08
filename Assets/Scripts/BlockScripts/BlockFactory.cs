using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFactory : MonoBehaviour
{
    private static BlockFactory _instance;
    public static BlockFactory Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(BlockFactory)) as BlockFactory;
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
    public GameObject CreateBlock(string name, Transform tilePosition)
    {
        GameObject obj = Instantiate(Resources.Load("blocks/" + name) as GameObject, tilePosition.position + Vector3.up * 2, Quaternion.identity);
        // NavigationManager.Instance.UpdateNavMeshSurfaces();
        return obj;
    }
}
