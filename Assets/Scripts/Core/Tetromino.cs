using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tetromino : MonoBehaviour
{
    public Vector2Int[] cells;     // 블록 모양 정의
    public Vector2Int tetrominoPosition;    // 현재 grid 위치

    float fallTime;
    public float fallSpeed = 1f;

    PlayerInputActions inputActions;

    /// <summary>
    /// 현재 입력값을 저장하기 위한 변수
    /// </summary>
    Vector2 moveInput;

    Transform[] shapes;

    private void Awake()
    {
        inputActions = new PlayerInputActions();

        // Shape 태그를 가진 자식들 캐싱
        shapes = GetComponentsInChildren<Transform>();

        shapes = Array.FindAll(shapes, t => t.CompareTag("Shape"));
    }

    private void OnEnable()
    {
        inputActions.MoveInput.Enable();
        inputActions.MoveInput.Move.performed += OnMove;
        inputActions.MoveInput.AAA.performed += AAA;
    }

    private void OnDisable()
    {
        inputActions.MoveInput.Move.performed -= OnMove;
        inputActions.MoveInput.Disable();
    }

    private void AAA(InputAction.CallbackContext context)
    {
        Debug.Log("A 버튼 누름");
    }


    private void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log("OnMove 호출");

        //moveInput = context.ReadValue<Vector2>();

        Vector2 input = context.ReadValue<Vector2>();

        if (input.x < -0.5f)
        {
            Move(Vector2Int.left);
        }
        else if (input.x > 0.5f)
        {
            Move(Vector2Int.right);
        }
        else if (input.y < -0.5f)
        {
            Move(Vector2Int.down);
        }
        else if (input.y > 0.5f)
        {
            Rotate();
        }
    }


    void Start()
    {
        UpdateVisualPosition();
    }

    void Update()
    {
        //HandleInput();
        HandleFall();
    }

    /*void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            Move(Vector2Int.left);

        else if (Input.GetKeyDown(KeyCode.RightArrow))
            Move(Vector2Int.right);

        else if (Input.GetKeyDown(KeyCode.DownArrow))
            Move(Vector2Int.down);

        else if (Input.GetKeyDown(KeyCode.UpArrow))
            Rotate();
    }*/

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
        Vector2Int newPosition = tetrominoPosition + dir;

        if (Board.IsValidPosition(cells, newPosition))
        {
            tetrominoPosition = newPosition;
            UpdateVisualPosition();
            return true;
        }

        return false;
    }

    int shapeRotationZ = 0;

    void Rotate()
    {
        /*// 시계 방향 회전 (x,y → -y,x)
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = new Vector2Int(-cells[i].y, cells[i].x);
        }

        if (!Board.IsValidPosition(cells, tetrominoPosition))
        {
            // 실패 시 복구 (반시계 회전)
            for (int i = 0; i < cells.Length; i++)
            {
                cells[i] = new Vector2Int(cells[i].y, -cells[i].x);
            }
        }
        else
        {
            // 회전 성공 시 Shape 태그를 가진 자식들 같이 회전
            shapeRotationZ = (shapeRotationZ + 90) % 360;
            // 90, 180, 270, 0 반복

            Debug.Log(shapeRotationZ);

            foreach (Transform shape in shapes)
            {
                shape.localRotation = Quaternion.Euler(0, 0, shapeRotationZ);
                Debug.Log("Shape 회전");
            }
        }

        UpdateVisualPosition();*/




        // 회전
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = new Vector2Int(-cells[i].y, cells[i].x);
        }

        // Wall Kick 후보 위치
        Vector2Int[] kicks =
        {
        Vector2Int.zero,
        Vector2Int.right,
        Vector2Int.left,
        Vector2Int.up,
        new Vector2Int(2,0),
        new Vector2Int(-2,0)
    };

        bool rotated = false;

        foreach (Vector2Int kick in kicks)
        {
            if (Board.IsValidPosition(cells, tetrominoPosition + kick))
            {
                tetrominoPosition += kick;
                rotated = true;
                break;
            }
        }

        if (!rotated)
        {
            // 실패 시 회전 되돌리기
            for (int i = 0; i < cells.Length; i++)
            {
                cells[i] = new Vector2Int(cells[i].y, -cells[i].x);
            }
            return;
        }

        // 회전 성공 시 Shape 회전
        shapeRotationZ = (shapeRotationZ + 90) % 360;

        foreach (Transform shape in shapes)
        {
            shape.localRotation = Quaternion.Euler(0, 0, shapeRotationZ);
        }

        UpdateVisualPosition();
    }

    public void UpdateVisualPosition()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3 worldPos = new Vector3(
                (cells[i].x + tetrominoPosition.x + 0.5f) * Board.cellSize,
                (cells[i].y + tetrominoPosition.y + 0.5f) * Board.cellSize,
                0
            );

            transform.GetChild(i).position = worldPos;

            /*if (i == 0)
                Debug.Log("X = " + worldPos.x);*/
        }
    }

    void Lock()
    {
        Board.AddToGrid(transform, cells, tetrominoPosition);
        enabled = false;

        FindObjectOfType<Spawner>().SpawnNext();
    }
}