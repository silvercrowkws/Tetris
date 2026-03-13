using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 게임상태
/// </summary>
public enum GameState
{
    Ready = 0,                  // 준비 상태
    GameStart,                  // 게임 시작
    GameEnd,                    // 게임 끝
}

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// 현재 게임상태
    /// </summary>
    public GameState gameState = GameState.Ready;

    /// <summary>
    /// 현재 게임상태 변경시 알리는 프로퍼티
    /// </summary>
    public GameState GameState
    {
        get => gameState;
        set
        {
            if (gameState != value)
            {
                gameState = value;
                switch (gameState)
                {
                    case GameState.Ready:
                        Debug.Log("준비 상태");
                        break;
                    case GameState.GameStart:
                        Debug.Log("게임 시작 상태");
                        onGameStart?.Invoke();
                        break;
                    case GameState.GameEnd:
                        Debug.Log("게임 끝 상태");
                        onGameEnd?.Invoke();
                        break;
                }
            }
        }
    }

    // 게임상태 델리게이트
    public Action onGameStart;
    public Action onGameEnd;

    private int score;
    public int Score
    {
        get => score;
        set
        {
            score = value;

            // 점수가 바뀔 때마다 등록된 함수(UI 갱신 등)를 모두 실행!
            onScoreChange?.Invoke(score, currentClearLineCount);
            Debug.Log($"점수 변경 : {Score}");
        }
    }

    public Action<int, int> onScoreChange;

    private int level = 1;
    public int Level
    {
        get => level;
        set
        {
            level = value;

            // 점수가 바뀔 때마다 등록된 함수(UI 갱신 등)를 모두 실행!
            onLevelChange?.Invoke(level);
            Debug.Log($"레벨 변경 : {Level}");
        }
    }    

    public Action<int> onLevelChange;

    /// <summary>
    /// 레벨 시스템을 위해 지워진 줄을 누적하는 변수
    /// </summary>
    int lineCount = 0;

    /// <summary>
    /// 현재 지워진 줄의 개수
    /// </summary>
    int currentClearLineCount;

    /// <summary>
    /// 게임 레디 패널
    /// </summary>
    GameReadyPanel gameReadyPanel;

    private void Start()
    {
        Board.onLineClear += OnLineClear;
    }

    private void OnDisable()
    {
        Board.onLineClear -= OnLineClear;
    }

    private void OnLineClear(int clearLineCount)
    {
        currentClearLineCount = clearLineCount;     // 현재 지워진 줄 개수 전달
        Debug.Log($"한번에 지워진 줄 개수 : {clearLineCount}");
        lineCount += clearLineCount;        // 개수 누적

        // 한번에 지워진 줄 개수별로 점수 누적 및 처리
        switch (clearLineCount)
        {
            case 1:
                Score += 40;
                // 싱글
                //StartCoroutine(ShowEventImage(eventImages[1]));
                break;
            case 2:
                Score += 100;
                // 더블
                //StartCoroutine(ShowEventImage(eventImages[2]));
                break;
            case 3:
                Score += 300;
                // 트리플
                //StartCoroutine(ShowEventImage(eventImages[3]));
                break;
            case 4:
                Score += 1200;
                // 테트리스
                //StartCoroutine(ShowEventImage(eventImages[4]));
                break;
        }

        if (lineCount > 9)
        {
            lineCount -= 10;        // 만약 누적 개수가 10이상이면 10을 빼고
            Level += 1;                // 레벨 증가
        }

        /*// 레벨업 테스트용
        if (lineCount > 1)
        {
            lineCount -= 2;        // 만약 누적 개수가 10이상이면 10을 빼고
            Level += 1;                // 레벨 증가
        }*/
    }


}
