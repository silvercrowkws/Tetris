using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static int width = 10;
    public static int height = 20;

    public static Transform[,] grid = new Transform[width, height];

    // 좌표 반올림
    public static Vector2 Round(Vector2 pos)
    {
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

    // 보드 내부 체크
    public static bool InsideBoard(Vector2 pos)
    {
        return (int)pos.x >= 0 &&
               (int)pos.x < width &&
               (int)pos.y >= 0;
    }

    // 위치 유효성 검사
    public static bool IsValidPosition(Transform block)
    {
        foreach (Transform child in block)
        {
            Vector2 pos = Round(child.position);

            if (!InsideBoard(pos))
                return false;

            if (pos.y < height)
            {
                if (grid[(int)pos.x, (int)pos.y] != null)
                    return false;
            }
        }
        return true;
    }

    // 그리드에 등록
    public static void AddToGrid(Transform block)
    {
        foreach (Transform child in block)
        {
            Vector2 pos = Round(child.position);
            grid[(int)pos.x, (int)pos.y] = child;
        }

        ClearLines(); // ⭐ 블록 고정 후 라인 체크
    }

    // ==============================
    // 🔥 여기부터 라인 클리어 시스템
    // ==============================

    public static void ClearLines()
    {
        for (int y = 0; y < height; y++)
        {
            if (IsLineFull(y))
            {
                DeleteLine(y);
                ShiftDown(y);
                y--; // 같은 줄 다시 검사
            }
        }
    }

    public static bool IsLineFull(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] == null)
                return false;
        }
        return true;
    }

    public static void DeleteLine(int y)
    {
        for (int x = 0; x < width; x++)
        {
            Object.Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    public static void ShiftDown(int deletedY)
    {
        for (int y = deletedY + 1; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] != null)
                {
                    grid[x, y - 1] = grid[x, y];
                    grid[x, y] = null;

                    grid[x, y - 1].position += Vector3.down;
                }
            }
        }
    }
}

