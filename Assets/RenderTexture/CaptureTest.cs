using UnityEngine;

public class CaptureTest : MonoBehaviour
{
    public SpriteCapture capture;

    private void Awake()
    {
        Debug.Log("CaptureTest 는 동작 함");
    }

    private void Start()
    {
        capture.CaptureAndSave();
        Debug.Log("찍힘");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            capture.CaptureAndSave();
            Debug.Log("찍힘");
        }
    }
}