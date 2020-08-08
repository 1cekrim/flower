using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class NavigationManager : MonoBehaviour
{
    public NavMeshSurface[] surfaces;
    private static NavigationManager _instance;
    private NavMeshAgent agent;
    public NavMeshAgentCallbacks navMeshAgentCallbacks;
    public static NavigationManager Instance
    {
        get
        {
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
        agent = GameManager.Instance.playerObject.GetComponent<NavMeshAgent>();
        navMeshAgentCallbacks = agent.gameObject.GetComponent<NavMeshAgentCallbacks>();
    }

    public void UpdateNavMeshSurfaces()
    {
        foreach (var nav in surfaces)
        {
            nav.BuildNavMesh();
        }
    }

    public (int x, int z) ConvertWorld(float oriX, float oriZ)
    {
        int x = Mathf.CeilToInt(oriX);
        int z = Mathf.CeilToInt(oriZ);
        x = x % 2 == 0 ? x : x - 1;
        z = z % 2 == 0 ? z : z - 1;
        return (x, z);
    }

    public (int col, int row) ConvertGrid(float x, float z)
    {
        (int col, int row) = ConvertWorld(x, z);
        return (col / 2 + GameManager.Instance.mapCols / 2, row / 2 + GameManager.Instance.mapRows / 2);
    }

    public void RotateAgentToTarget(Transform target)
    {
        Transform player = GameManager.Instance.playerObject.transform;
        Vector3 direction = target.position - player.position;
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        player.DORotate(lookRotation.eulerAngles, 0.5f);
    }

    public void MoveToCoord(int col, int row, bool onlyAround = false)
    {
        if (GameManager.Instance.mapTile[row, col] == null)
        {
            return;
        }

        if (onlyAround || !GameManager.Instance.mapTile[row, col].canMove)
        {
            // 해당 칸이 올라갈 수 없는 칸이면, 가장 가까운 곳부터 시계방향으로 8방향 탐색
            // TODO: 8방향 다 막혀있으면 다이얼로그 띄우기
            Func<int, int> Norm = k => (k == 0 ? 0 : (k > 0 ? 1 : -1));
            (int playerCol, int playerRow) = ConvertGrid(GameManager.Instance.playerObject.transform.position.x, GameManager.Instance.playerObject.transform.position.z);
            int dx = (int)(playerCol - col);
            int dz = (int)(playerRow - row);
            dx = Norm(dx);
            dz = Norm(dz);
            for (int i = 0; i < 8; ++i)
            {
                if (GameManager.Instance.mapTile[row + dz, col + dx].canMove)
                {
                    FloorTile target = GameManager.Instance.mapTile[row, col];
                    navMeshAgentCallbacks.CompleteEvent.AddListener(() =>
                    {
                        Debug.Log(target.transform);
                        RotateAgentToTarget(target.transform);
                    });
                    row += dz;
                    col += dx;
                    break;
                }
                int tx = Norm(dx + dz);
                dz = Norm(dz - dx);
                dx = tx;
            }
        }

        agent.SetDestination(GameManager.Instance.mapTile[row, col].transform.position + new Vector3(0, 1, 0));
        navMeshAgentCallbacks.StartMove();
    }
}
