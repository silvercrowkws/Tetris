using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static int width = 10;
    public static int height = 20;

    public static float cellSize = 0.205f;

    [Header("Game Grid Debug")]
    public Material lineMaterial;   // Unlit Color 머티리얼
    public Color gridColor = new Color(1f, 0.8f, 0.6f, 1f); // 살구색

    Transform gridParent;   // GridLines 부모

    // 보드 데이터 (정수 그리드 기반)
    public static Transform[,] grid = new Transform[width, height];

    void Start()
    {
        CreateGridParent();
        DrawGameGrid();
    }

    void CreateGridParent()
    {
        // 이미 있다면 재사용
        Transform existing = transform.Find("GridLines");

        if (existing != null)
        {
            gridParent = existing;
            return;
        }

        GameObject parentObj = new GameObject("GridLines");
        parentObj.transform.SetParent(transform);
        parentObj.transform.localPosition = Vector3.zero;

        gridParent = parentObj.transform;
    }

    /// <summary>
    /// 보드 범위 체크
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static bool InsideBoard(Vector2Int pos)
    {
        return pos.x >= 0 &&
               pos.x < width &&
               pos.y >= 0 &&
               pos.y < height;
    }

    /// <summary>
    /// 위치 유효성 검사,
    /// 이동 가능 여부 판단 함수
    /// </summary>
    /// <param name="cells"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public static bool IsValidPosition(Vector2Int[] cells, Vector2Int position)
    {
        foreach (Vector2Int cell in cells)
        {
            Vector2Int tilePos = cell + position;

            if (!InsideBoard(tilePos))
                return false;

            if (grid[tilePos.x, tilePos.y] != null)
                return false;
        }

        return true;
    }

    /// <summary>
    /// 블록이 바닥에 닿아 고정될 때 호출되는 함수
    /// </summary>
    /// <param name="block"></param>
    /// <param name="cells"></param>
    /// <param name="position"></param>
    public static void AddToGrid(Transform block, Vector2Int[] cells, Vector2Int position)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            Vector2Int tilePos = cells[i] + position;

            if (tilePos.y >= height)
                continue;

            grid[tilePos.x, tilePos.y] = block.GetChild(i);
        }

        ClearLines();
    }

    /// <summary>
    /// 전체 행(y)을 순회하며 줄이 꽉 찼는지 검사하고, 찼다면 DeleteLine과 ShiftDown을 실행하는 함수
    /// </summary>
    public static void ClearLines()
    {
        for (int y = 0; y < height; y++)
        {
            if (IsLineFull(y))
            {
                DeleteLine(y);
                ShiftDown(y);
                y--;
            }
        }

        // 줄 삭제가 모두 끝난 뒤, 빈 테트로미노 정리(한 프레임 내에 일어나서 다음 블록이 고정될 때 처리됨)
        CleanupOrphanedTetrominos();
    }

    /// <summary>
    /// 특정 행(y)의 모든 x 좌표에 블록이 있는지 확인하고, 하나라도 비어있으면 false를 반환하는 함수
    /// </summary>
    /// <param name="y"></param>
    /// <returns></returns>
    static bool IsLineFull(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] == null)
                return false;
        }

        return true;
    }

    /// <summary>
    /// 해당 행의 블록을 화면에서 Destroy하고, grid[,] 배열 데이터를 null로 초기화하는 함수
    /// </summary>
    /// <param name="y"></param>
    static void DeleteLine(int y)
    {
        for (int x = 0; x < width; x++)
        {
            Object.Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    /// <summary>
    /// 줄이 사라지면 그 위에 있던 블록들이 아래로 내려와야 함.
    /// 사라진 줄보다 위에 있는 모든 블록을 한 칸씩 내리고, grid[,] 배열의 데이터 위치도 갱신하는 함수
    /// </summary>
    /// <param name="deletedY"></param>
    static void ShiftDown(int deletedY)
    {
        for (int y = deletedY + 1; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] != null)
                {
                    grid[x, y - 1] = grid[x, y];
                    grid[x, y] = null;

                    grid[x, y - 1].position += Vector3.down * cellSize;
                }
            }
        }
    }

    /// <summary>
    /// 빈 테트로미노 정리 함수
    /// </summary>
    static void CleanupOrphanedTetrominos()
    {
        // 씬에 존재하는 모든 Tetromino 컴포넌트를 찾고
        Tetromino[] allTetrominos = Object.FindObjectsByType<Tetromino>(FindObjectsSortMode.None);

        foreach (Tetromino t in allTetrominos)
        {
            // Tetromino가 가지고 있는 실제 자식(Shape 태그 등) 개수를 세어서
            // GetComponentsInChildren은 자기 자신도 포함하므로, 1보다 크면 자식이 있다는 뜻
            if (t.GetComponentsInChildren<Transform>().Length <= 1)
            {
                Destroy(t.gameObject);
            }
        }
    }

    /// <summary>
    /// 게임 실행하지 않아도 Scene뷰에서 보드 경계선 보게 함
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;

        for (int x = 0; x <= width; x++)
        {
            Gizmos.DrawLine(
                new Vector3(x * cellSize, 0, 0),
                new Vector3(x * cellSize, height * cellSize, 0)
            );
        }

        for (int y = 0; y <= height; y++)
        {
            Gizmos.DrawLine(
                new Vector3(0, y * cellSize, 0),
                new Vector3(width * cellSize, y * cellSize, 0)
            );
        }
    }

    /// <summary>
    /// CreateLine으로 라인랜더러 생성 함수(Start에서)
    /// </summary>
    void DrawGameGrid()
    {
        // 세로선
        for (int x = 0; x <= width; x++)
        {
            CreateLine(
                new Vector3(x * cellSize, 0, 0),
                new Vector3(x * cellSize, height * cellSize, 0)
            );
        }

        // 가로선
        for (int y = 0; y <= height; y++)
        {
            CreateLine(
                new Vector3(0, y * cellSize, 0),
                new Vector3(width * cellSize, y * cellSize, 0)
            );
        }
    }

    /// <summary>
    /// 라인랜더러로 보드 그리는 함수
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    void CreateLine(Vector3 start, Vector3 end)
    {
        GameObject lineObj = new GameObject("GridLine");
        lineObj.transform.SetParent(gridParent);
        Vector3 spawnLinePos = new Vector3(0, 0, -0.1f);
        lineObj.transform.localPosition = spawnLinePos;

        LineRenderer lr = lineObj.AddComponent<LineRenderer>();

        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        lr.startWidth = 0.01f;
        lr.endWidth = 0.01f;

        lr.material = lineMaterial;
        lr.startColor = gridColor;
        lr.endColor = gridColor;

        lr.useWorldSpace = false;
        lr.sortingOrder = 100;
    }
}