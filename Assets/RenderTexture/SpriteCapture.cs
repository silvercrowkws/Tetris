/*using UnityEngine;

public class SpriteCapture : MonoBehaviour
{
    public Camera captureCamera;
    public RenderTexture renderTexture;

    public Sprite Capture()
    {
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = renderTexture;

        captureCamera.Render();

        Texture2D tex = new Texture2D(
            renderTexture.width,
            renderTexture.height,
            TextureFormat.RGBA32,
            false
        );

        tex.ReadPixels(
            new Rect(0, 0, renderTexture.width, renderTexture.height),
            0,
            0
        );

        tex.Apply();

        RenderTexture.active = currentRT;

        Sprite sprite = Sprite.Create(
            tex,
            new Rect(0, 0, tex.width, tex.height),
            new Vector2(0.5f, 0.5f)
        );

        return sprite;
    }
}*/

using UnityEngine;
using System.IO;

public class SpriteCapture : MonoBehaviour
{
    public Camera captureCamera;
    public RenderTexture renderTexture;

    public Sprite CaptureAndSave()
    {
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = renderTexture;

        captureCamera.Render();

        Texture2D tex = new Texture2D(
            renderTexture.width,
            renderTexture.height,
            TextureFormat.RGBA32,
            false
        );

        tex.ReadPixels(
            new Rect(0, 0, renderTexture.width, renderTexture.height),
            0,
            0
        );

        tex.Apply();

        RenderTexture.active = currentRT;

        // PNG 저장
        byte[] png = tex.EncodeToPNG();
        File.WriteAllBytes(
            Application.dataPath + "/CapturedSprite.png",
            png
        );

        // Sprite 생성
        Sprite sprite = Sprite.Create(
            tex,
            new Rect(0, 0, tex.width, tex.height),
            new Vector2(0.5f, 0.5f)
        );

        return sprite;
    }
}