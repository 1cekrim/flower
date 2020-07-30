using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationManager : MonoBehaviour
{
    public NavMeshSurface[] surfaces;
    private static NavigationManager _instance;
    public static NavigationManager Instance
    {
        get {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(NavigationManager)) as NavigationManager;
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

    public void UpdateNavMeshSurfaces()
    {
        foreach (var nav in surfaces)
        {
            nav.BuildNavMesh();
        }
    }
}
