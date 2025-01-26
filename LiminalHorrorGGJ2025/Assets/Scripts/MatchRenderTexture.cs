using UnityEngine;

public class MatchRenderTexture : MonoBehaviour
{
    public Camera renderCamera;
    public RenderTexture renderTexture;

    void Start()
    {
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

        renderTexture.width = Screen.width;
        renderTexture.height = Screen.height;
        renderTexture.antiAliasing = 1;
        renderTexture.filterMode = FilterMode.Bilinear;

        renderCamera.targetTexture = renderTexture;
    }

    void OnDestroy()
    {
        if (renderTexture != null)
        {
            renderTexture.Release();
        }
    }
}