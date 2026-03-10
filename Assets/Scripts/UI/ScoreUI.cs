using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    TextMeshProUGUI lvText;

    TextMeshProUGUI scoreText;

    //Board board;

    int score = 0;
    int lv = 1;

    /// <summary>
    /// 레벨 시스템을 위해 지워진 줄을 누적하는 변수
    /// </summary>
    int lineCount = 0;

    private void Awake()
    {
        lvText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        scoreText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        //board = FindAnyObjectByType<Board>();
        Board.onLineClear += AddLineClear;
    }

    private void OnDisable()
    {
        Board.onLineClear -= AddLineClear;
    }

    private void AddLineClear(int clearLineCount)
    {
        Debug.Log($"한번에 지워진 줄 개수 : {clearLineCount}");
        lineCount += clearLineCount;        // 개수 누적

        // 점수 누적
        switch( clearLineCount )
        {
            case 1:
                score += 40;
                break;
            case 2:
                score += 100;
                break;
            case 3:
                score += 300;
                break;
            case 4:
                score += 1200;
                break;
        }

        Debug.Log($"점수: {score}");
        ScoreChange();

        if(lineCount > 9)
        {
            lineCount -= 10;        // 만약 누적 개수가 10이상이면 10을 빼고
            lv += 1;                // 레벨 증가
            LVChange();
        }
    }

    /// <summary>
    /// 점수 텍스트 수정 함수
    /// </summary>
    private void ScoreChange()
    {
        scoreText.text = score.ToString("N0");
    }

    /// <summary>
    /// 레벨 텍스트 수정 함수 및 난이도 조절 함수
    /// </summary>
    private void LVChange()
    {
        // LV UP!! 같은 표현 추가하고
        lvText.text = lv.ToString();

        // 난이도 조절은 어떻게 한담?
    }
}
