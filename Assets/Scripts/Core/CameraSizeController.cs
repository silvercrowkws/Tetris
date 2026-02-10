using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraSizeController : MonoBehaviour
{
    const float referenceSize = 5f;

    void Awake()
    {
        Camera cam = GetComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = referenceSize;
    }
}
