using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlButtons : MonoBehaviour
{
    Button[] buttons;
    Spawner spawner;

    /// <summary>
    /// 어떤 버튼이 클릭되었는지 전달하는 델리게이트
    /// </summary>
    public Action<int> onButtonClick;

    private void Awake()
    {
        buttons = new Button[4];

        for(int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i] = transform.GetChild(i).GetComponent<Button>();
            buttons[i].onClick.AddListener(() => OnButtonClicked(index));
        }
    }

    private void Start()
    {
        spawner = FindAnyObjectByType<Spawner>();
    }

    void OnButtonClicked(int index)
    {
        if (spawner.currentTetromino == null)
        {
            return;
        }

        /*Tetromino t = spawner.currentTetromino;

        switch (index)
        {
            case 0:
                Debug.Log("회전");
                t.Rotate();
                break;

            case 1:
                Debug.Log("아래");
                t.Move(Vector2Int.down);
                break;

            case 2:
                Debug.Log("왼쪽");
                t.Move(Vector2Int.left);
                break;

            case 3:
                Debug.Log("오른쪽");
                t.Move(Vector2Int.right);
                break;
        }*/

        // 🔥 인덱스만 넘겨주면 Tetromino 내부에서 알아서 처리합니다.
        spawner.currentTetromino.onButtonClick?.Invoke(index);
    }
}
