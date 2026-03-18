using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEndPAnel : MonoBehaviour
{
    GameManager gameManager;

    CanvasGroup canvasGroup;

    Button reStartButton;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        reStartButton = transform.GetComponentInChildren<Button>();
        reStartButton.onClick.AddListener(ReStart);

        CanvasGroupControl(false);
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.onGameEnd += OnGameEnd;
    }

    private void OnDisable()
    {
        gameManager.onGameEnd -= OnGameEnd;
    }

    private void OnGameEnd()
    {
        CanvasGroupControl(true);
    }

    /// <summary>
    /// true: 알파1
    /// </summary>
    /// <param name="tf"></param>
    private void CanvasGroupControl(bool tf)
    {
        if (tf)
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    private void ReStart()
    {
        Debug.Log("재시작 버튼 클릭");

        gameManager.GameState = GameState.Ready;
        SceneManager.LoadScene(0);
    }
}
