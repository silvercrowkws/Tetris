using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    TextMeshProUGUI lvText;

    TextMeshProUGUI scoreText;

    //Board board;

    /*/// <summary>
    /// 레벨 시스템을 위해 지워진 줄을 누적하는 변수
    /// </summary>
    int lineCount = 0;*/

    /// <summary>
    /// 레벨업, 줄 파괴 시 보여줄 이미지
    /// </summary>
    public GameObject[] eventImages;

    GameManager gameManager;

    private void Awake()
    {
        lvText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        scoreText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.onScoreChange += ScoreChange;
        gameManager.onLevelChange += LVChange;
    }

    private void OnDisable()
    {
        gameManager.onScoreChange -= ScoreChange;
        gameManager.onLevelChange -= LVChange;
    }

    /// <summary>
    /// 점수 텍스트 수정 함수
    /// </summary>
    private void ScoreChange(int score, int clearLineCount)
    {
        StartCoroutine(ShowEventImage(eventImages[clearLineCount]));

        scoreText.text = score.ToString("N0");
    }

    /// <summary>
    /// 레벨 텍스트 수정 함수 및 난이도 조절 함수
    /// </summary>
    private void LVChange(int level)
    {
        // LV UP!! 같은 표현 추가
        StartCoroutine(ShowEventImage(eventImages[0]));
        lvText.text = level.ToString();

        // 난이도 조절은 어떻게 한담? => 게임 매니저에서 처리
    }

    IEnumerator ShowEventImage(GameObject obj)
    {
        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();

        obj.SetActive(true);
        canvasGroup.alpha = 1f;

        // 2초 동안 그대로 유지
        yield return new WaitForSeconds(2f);

        float time = 0f;
        float duration = 1f;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = 1f - (time / duration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        obj.SetActive(false);
    }
}
