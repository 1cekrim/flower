using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitIngameMap : MonoBehaviour
{
    public GameObject[] tileObjects = new GameObject[2];
    public GameObject seaObject;
    public GameObject mapParent;
    public int gridRows;
    public int gridCols;
    public int cellSize;
    public int waterPadding;
    private GameObject[,] tiles;

    public void CreateTile()
    {
        // TODO: 맵을 처음 생성할 때는 특수효과 넣기
        Debug.Log("InitIngameMap::CreateTile");
        tiles = new GameObject[gridRows, gridCols];
        Quaternion seaRotation = Quaternion.identity;
        seaRotation.eulerAngles = new Vector3(90, 0, 0);
        for (int i = -waterPadding; i < gridRows + waterPadding; ++i)
        {
            for (int j = -waterPadding; j < gridCols + waterPadding; ++j)
            {
                if (i >= 0 && i < gridRows && j >= 0 && j < gridCols)
                {
                    tiles[i, j] = Instantiate(tileObjects[(i + j) % 2], new Vector3(j, 0, i) * cellSize, Quaternion.identity) as GameObject;
                    tiles[i, j].transform.parent = mapParent.transform;
                }
                else
                {
                    var obj = Instantiate(seaObject, new Vector3(j, 0, i) * cellSize, seaRotation) as GameObject;
                    obj.transform.parent = mapParent.transform;
                }
            }
        }
    }
}
