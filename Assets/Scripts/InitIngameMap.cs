using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitIngameMap : MonoBehaviour
{
    public GameObject[] tileObjects = new GameObject[2];
    public GameObject waterObject;
    public GameObject mapParent;
    public int gridRows;
    public int gridCols;
    public int cellSize;

    private int[] dx = {0, -1, 1, 0};
    private int[] dy = {1, 0, 0, -1};

    private void checkMap(bool[,] tMap, bool[,] map, int x, int y)
    {
        tMap[y, x] = false;
        map[y, x] = true;
        for (int i = 0; i < 4; ++i)
        {
            int nx = x + dx[i];
            int ny = y + dy[i];
            if (nx >= 0 && nx < gridCols && ny >= 0 && ny < gridRows && tMap[ny, nx])
            {
                checkMap(tMap, map, nx, ny);
            }
        }
    }

    public void CreateTile()
    {
        // TODO: 맵을 처음 생성할 때는 특수효과 넣기
        Debug.Log("InitIngameMap::CreateTile");
        GameManager.Instance.mapTile = new FloorTile[gridRows, gridCols];
        Quaternion waterRotation = Quaternion.identity;
        waterRotation.eulerAngles = new Vector3(90, 0, 0);
        Vector3 center = new Vector3((int)(gridCols / 2), 0, (int)(gridRows / 2)) * cellSize;
        float maxDist = Mathf.Sqrt(gridCols * gridCols + gridRows * gridRows);
        bool[,] tMap = new bool[gridRows, gridCols];
        bool[,] map = new bool[gridRows, gridCols];
        int seed = Random.Range(0, 1000);
        for (int i = 0; i < gridRows; ++i)
        {
            for (int j = 0; j < gridCols; ++j)
            {
                float dist = Mathf.Sqrt((i - gridRows/2) * (i - gridRows/2) + (j - gridCols/2) * (j - gridCols/2));
                dist /= maxDist / 2;
                var perlin = Mathf.PerlinNoise((float)i / (float)gridRows * 12 + seed, (float)j / gridCols * 12 + seed) - dist + .05f;
                
                if (perlin > .05f)
                {
                    tMap[i, j] = true;
                }
                else
                {
                    tMap[i, j] = false;
                }
            }
        }

        checkMap(tMap, map, (int)(gridCols / 2), (int)(gridRows / 2));

        for (int i = 0; i < gridRows; ++i)
        {
            for (int j = 0; j < gridCols; ++j)
            {
                if (map[i, j])
                {
                    var obj = Instantiate(tileObjects[(i + j) % 2], new Vector3(j, 0, i) * cellSize - center, Quaternion.identity) as GameObject;
                    obj.transform.parent = mapParent.transform;

                    FloorTile ft = obj.AddComponent<FloorTile>();
                    ft.canMove = true;
                    GameManager.Instance.mapTile[i, j] = ft;
                }
                else
                {
                    var obj = Instantiate(waterObject, new Vector3(j, 0, i) * cellSize - center, waterRotation) as GameObject;
                    obj.transform.parent = mapParent.transform;

                    FloorTile ft = obj.AddComponent<FloorTile>();
                    ft.canMove = false;
                    GameManager.Instance.mapTile[i, j] = ft;
                }
                
            }
        }
    }
}
