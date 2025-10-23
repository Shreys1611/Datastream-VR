using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public static SceneFader Instance;

    [Header("Fade Settings")]
    public float fadeDuration = 1f;
    public Image fadeImage; // The black image with CanvasGroup or Image

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; // Hook into scene load
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Start each scene fully black → fade in
        fadeImage.canvasRenderer.SetAlpha(1f);
        StartCoroutine(FadeIn());
    }

    public void LoadSceneByIndex(int index)
    {
        StartCoroutine(FadeOutAndLoad(index));
    }

    public void LoadSceneByName(string sceneName)
    {
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    private IEnumerator FadeOutAndLoad(int sceneIndex)
    {
        yield return StartCoroutine(FadeOut());
        SceneManager.LoadScene(sceneIndex);
    }

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        yield return StartCoroutine(FadeOut());
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Whenever a scene finishes loading, fade back in
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeOut()
    {
        fadeImage.CrossFadeAlpha(1f, fadeDuration, false);
        yield return new WaitForSeconds(fadeDuration);
    }

    private IEnumerator FadeIn()
    {
        fadeImage.CrossFadeAlpha(0f, fadeDuration, false);
        yield return new WaitForSeconds(fadeDuration);
    }
}
 