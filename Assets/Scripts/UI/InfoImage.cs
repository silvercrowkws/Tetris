using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoImage : MonoBehaviour
{
    Image burpImage;

    private void Awake()
    {
        burpImage = transform.GetChild(0).GetComponent<Image>();
        burpImage.gameObject.SetActive(false);
    }

    private void Start()
    {
        Board.onLineClear += OnBurp;
    }

    private void OnDisable()
    {
        Board.onLineClear -= OnBurp;
    }

    private void OnBurp(int clearLineCount)
    {
        StartCoroutine(BurpCoroutine());
    }

    IEnumerator BurpCoroutine()
    {
        burpImage.gameObject.SetActive(true);

        yield return new WaitForSeconds(1);

        burpImage.gameObject.SetActive(false);
    }
}
