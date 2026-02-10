using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_01_Sprites : TestBase
{
    public GameObject rotationObject;

    private int rotationStep = 0; // 0,1,2,3

    private void Start()
    {
        
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        if (!rotationObject.activeSelf)
        {
            rotationObject.SetActive(true);
        }

        rotationStep = (rotationStep + 1) % 4; // 0~3 반복
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
}
