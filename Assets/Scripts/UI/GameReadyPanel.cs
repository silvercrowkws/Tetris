using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameReadyPanel : MonoBehaviour
{
    Button startButton;

    public Action<bool> onGameReadyPanelGameStart;

    CanvasGroup canvasGroup;

    private void Awake()
    {
        startButton = transform.GetChild(0).GetComponent<Button>();
        startButton.onClick.AddListener(OnStartButton);

        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnStartButton()
    {
        Debug.Log("게임 시작 버튼 클릭");
        CanvasGroupCantroll(false);
        onGameReadyPanelGameStart?.Invoke(true);
    }

    /// <summary>
    /// 캔버스 그룹 조절 함수
    /// </summary>
    /// <param name="tf">t : 활성화처리, f : 비활성화 처리</param>
    private void CanvasGroupCantroll(bool tf)
    {
        if(tf)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
