using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_02_Board : TestBase
{
    public GameObject rotationObject;

    private int rotationStep = 0;       // 0,1,2,3

    public int[] index;

    public Spawner spawner;

    public bool isCustom;


    protected override void Awake()
    {
        base.Awake();
        // (참고: TestBase에 Awake가 정의되어 있다면 protected override void Awake() 로 작성하고 base.Awake()를 호출하세요)

        //Spawner spawner = FindFirstObjectByType<Spawner>(); // 씬에서 Spawner 찾기 (Unity 2023 이상 권장, 구버전은 FindObjectOfType)

        // 인스펙터에 설정된 배열이 존재할 때만 전달
        if (isCustom)
        {
            if (spawner != null && index != null && index.Length > 0)
            {
                spawner.SetCustomSequence(index);
            }
        }
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        if (!rotationObject.activeSelf)
        {
            rotationObject.SetActive(true);
        }

        rotationStep = (rotationStep + 1) % 4;      // 0~3 반복
        float angle = rotationStep * 90f;

        rotationObject.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        if(rotationObject.activeSelf)
        {
            rotationObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            rotationObject.SetActive(false);
        }
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {

    }
}
