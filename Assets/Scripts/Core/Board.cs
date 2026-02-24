using UnityEngine;

public class Board : MonoBehaviour
{
    public static int width = 10;
    public static int height = 20;

    public static float cellSize = 0.205f;

    // 보드 데이터 (정수 그리드 기반)
    public static Transform[,] grid = new Transform[width, height];

    // 보드 범위 체크
    public static bool InsideBoard(Vector2Int pos)
    {
        return pos.x >= 0 &&
               pos.x < width &&
               pos.y >= 0 &&
               pos.y < height;
    }

    // 위치 유효성 검사
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

    // 블록 고정
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

    // 줄 삭제
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
    }

    static bool IsLineFull(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] == null)
                return false;
        }

        return true;
    }

    static void DeleteLine(int y)
    {
        for (int x = 0; x < width; x++)
        {
            Object.Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

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

    // 디버그용 그리드 표시
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
}