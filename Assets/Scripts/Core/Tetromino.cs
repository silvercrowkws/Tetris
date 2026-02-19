using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    float fallTime = 0f;
    public float fallSpeed = 1f;
    
    void Update()
    {
        HandleInput();
        HandleFall();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            Move(Vector3.left);

        else if (Input.GetKeyDown(KeyCode.RightArrow))
            Move(Vector3.right);

        else if (Input.GetKeyDown(KeyCode.DownArrow))
            Move(Vector3.down);

        else if (Input.GetKeyDown(KeyCode.UpArrow))
            Rotate();
    }

    void HandleFall()
    {
        if (Time.time - fallTime >= fallSpeed)
        {
            Move(Vector3.down);
            fallTime = Time.time;
        }
    }

    void Move(Vector3 dir)
    {
        transform.position += dir;

        if (!Board.IsValidPosition(transform))
        {
            transform.position -= dir;

            // 아래로 이동 실패 → 블록 고정
            if (dir == Vector3.down)
            {
                Board.AddToGrid(transform);
                enabled = false;

                // ⭐ 위에 공간이 없으면 게임오버
                foreach (Transform child in transform)
                {
                    if (child.position.y >= Board.height - 1)
                    {
                        // 게임 오버 처리
                        Debug.Log("게임 오버");
                    }
                }

                FindObjectOfType<Spawner>().SpawnNext();
            }
        }
    }

    void Rotate()
    {
        transform.Rotate(0, 0, 90);

        if (!Board.IsValidPosition(transform))
            transform.Rotate(0, 0, -90);
    }
}
