// PacketFader.cs
using UnityEngine;
using System.Collections;

public class PacketFader : MonoBehaviour
{
    private Renderer objectRenderer;

    void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        // Ensure the material can be made transparent
        if (objectRenderer != null)
        {
            objectRenderer.material.ToFadeMode();
        }
    }

    // Public function to start the fade-out process
    public void StartFadeOut(float duration)
    {
        if (objectRenderer != null)
        {
            StartCoroutine(FadeOutCoroutine(duration));
        }
        else
        {
            Destroy(gameObject); // Fallback if no renderer is found
        }
    }

    private IEnumerator FadeOutCoroutine(float duration)
    {
        float counter = 0;
        Color startColor = objectRenderer.material.color;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(startColor.a, 0, counter / duration);
            objectRenderer.material.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        Destroy(gameObject);
    }
}

// This is a helper extension method to change a material's rendering mode to support transparency
public static class MaterialExtensions
{
    public static void ToFadeMode(this Material material)
    {
        material.SetOverrideTag("RenderType", "Transparent");
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }
}