using UnityEngine;

public class Tetromino : MonoBehaviour
{
    public Vector2Int[] cells;     // 블록 모양 정의
    public Vector2Int position;    // 현재 grid 위치

    float fallTime;
    public float fallSpeed = 1f;

    void Start()
    {
        UpdateVisualPosition();
    }

    void Update()
    {
        HandleInput();
        HandleFall();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            Move(Vector2Int.left);

        else if (Input.GetKeyDown(KeyCode.RightArrow))
            Move(Vector2Int.right);

        else if (Input.GetKeyDown(KeyCode.DownArrow))
            Move(Vector2Int.down);

        else if (Input.GetKeyDown(KeyCode.UpArrow))
            Rotate();
    }

    void HandleFall()
    {
        if (Time.time - fallTime >= fallSpeed)
        {
            if (!Move(Vector2Int.down))
            {
                Lock();
            }

            fallTime = Time.time;
        }
    }

    bool Move(Vector2Int dir)
    {
        Vector2Int newPosition = position + dir;

        if (Board.IsValidPosition(cells, newPosition))
        {
            position = newPosition;
            UpdateVisualPosition();
            return true;
        }

        return false;
    }

    void Rotate()
    {
        // 시계 방향 회전 (x,y → -y,x)
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = new Vector2Int(-cells[i].y, cells[i].x);
        }

        if (!Board.IsValidPosition(cells, position))
        {
            // 실패 시 복구 (반시계 회전)
            for (int i = 0; i < cells.Length; i++)
            {
                cells[i] = new Vector2Int(cells[i].y, -cells[i].x);
            }
        }

        UpdateVisualPosition();
    }

    public void UpdateVisualPosition()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3 worldPos = new Vector3(
                (cells[i].x + position.x - 0.5f) * Board.cellSize,
                (cells[i].y + position.y - 0.5f) * Board.cellSize,
                0
            );

            transform.GetChild(i).position = worldPos;

            /*if (i == 0)
                Debug.Log("X = " + worldPos.x);*/
        }
    }

    void Lock()
    {
        Board.AddToGrid(transform, cells, position);
        enabled = false;

        FindObjectOfType<Spawner>().SpawnNext();
    }
}