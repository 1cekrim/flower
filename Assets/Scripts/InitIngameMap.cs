using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitIngameMap : MonoBehaviour
{
    public GameObject[] tileObjects = new GameObject[2];
    public GameObject waterObject;
    public GameObject soilObject;
    public GameObject mapParent;
    public int gridRows;
    public int gridCols;
    public int cellSize;

    public class Pair<T, U>
    {
        public Pair()
        {
        }

        public Pair(T first, U second)
        {
            this.First = first;
            this.Second = second;
        }

        public T First { get; set; }
        public U Second { get; set; }
    }

    public void CreateTile()
    {
        // TODO: 맵을 처음 생성할 때는 특수효과 넣기
        Debug.Log("InitIngameMap::CreateTile");
        GameManager.Instance.mapTile = new FloorTile[gridRows, gridCols];
        GameManager.Instance.mapCols = gridCols;
        GameManager.Instance.mapRows = gridRows;
        Quaternion waterRotation = Quaternion.identity;
        waterRotation.eulerAngles = new Vector3(90, 0, 0);
        Vector3 center = new Vector3((int)(gridCols / 2), 0, (int)(gridRows / 2)) * cellSize;
        float maxDist = Mathf.Sqrt(gridCols * gridCols + gridRows * gridRows);
        TileEnum[,] map = new TileEnum[gridRows, gridCols];

        // Perlin noise 값에, 섬 중심으로부터의 거리에 비례하는 dist를 뺀 후 .05f를 더한다
        // 이 값이 0.5f보다 크면 땅이라는 표시로 map[i, j]를 true로, 아니면 물이라는 표시로 map[i, j]를 false로 한다
        int seed = Random.Range(0, 1000);
        for (int i = 0; i < gridRows; ++i)
        {
            for (int j = 0; j < gridCols; ++j)
            {
                float dist = Mathf.Sqrt((i - gridRows / 2) * (i - gridRows / 2) + (j - gridCols / 2) * (j - gridCols / 2));
                dist /= maxDist / 2.5f;
                var perlin = Mathf.PerlinNoise((float)i / (float)gridRows * 8 + seed, (float)j / gridCols * 8 + seed) - dist + .05f;

                if (perlin > .2f)
                {
                    map[i, j] = TileEnum.grass;
                }
                else if (perlin > .05f)
                {
                    map[i, j] = TileEnum.soil;
                }
                else
                {
                    map[i, j] = TileEnum.water;
                }
            }
        }

        // 위 알고리즘에서 섬 중심은 dist가 0이므로, Perlin Noise와 상관없이 항상 땅이다
        // 그러므로 섬 중심으로부터 BFS 탐색을 진행해, 섬 중심과 떨어져 있는 땅을 제외하고 Instantiate한다
        // 땅과 닿아있는 물 부분에는 waterObject를 Instantiate해 상호작용이 가능하게 한다
        Queue<Pair<int, int>> q = new Queue<Pair<int, int>>();
        q.Enqueue(new Pair<int, int>((int)(gridCols / 2), (int)(gridRows / 2)));
        // map[(int)(gridRows / 2), (int)(gridCols / 2)] = false;
        bool[,] check = new bool[gridRows, gridCols];
        check.Initialize();
        check[(int)(gridRows / 2), (int)(gridCols / 2)] = true;
        int[] dx = { 0, -1, 1, 0 };
        int[] dy = { 1, 0, 0, -1 };
        while (q.Count > 0)
        {
            var top = q.Dequeue();
            int x = top.First;
            int y = top.Second;
            {
                GameObject obj;
                if (map[y, x] == TileEnum.grass)
                {
                    obj = Instantiate(tileObjects[(y + x) % 2], new Vector3(x, 0, y) * cellSize - center, Quaternion.identity) as GameObject;
                }
                else
                {
                    obj = Instantiate(soilObject, new Vector3(x, 0, y) * cellSize - center, Quaternion.identity) as GameObject;
                }
                obj.transform.parent = mapParent.transform;
                FloorTile ft = obj.AddComponent<FloorTile>();
                ft.canMove = true;
                GameManager.Instance.mapTile[y, x] = ft;
            }

            for (int i = 0; i < 4; ++i)
            {
                int nx = x + dx[i];
                int ny = y + dy[i];
                if (nx >= 0 && nx < gridCols && ny >= 0 && ny < gridRows && !check[ny, nx])
                {
                    check[ny, nx] = true;
                    if (map[ny, nx] == TileEnum.grass || map[ny, nx] == TileEnum.soil)
                    {
                        q.Enqueue(new Pair<int, int>(nx, ny));
                    }
                    else
                    {
                        var obj = Instantiate(waterObject, new Vector3(nx, 0, ny) * cellSize - center, waterRotation) as GameObject;
                        obj.transform.parent = mapParent.transform;
                        FloorTile ft = obj.AddComponent<FloorTile>();
                        ft.canMove = false;
                        GameManager.Instance.mapTile[ny, nx] = ft;
                    }
                }
            }
        }

        // 맵 생성이 완료된 뒤에 NavMesh를 업데이트함
        NavigationManager.Instance.UpdateNavMeshSurfaces();
    }
}
