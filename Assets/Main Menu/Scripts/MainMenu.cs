using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1.5f;

    // --- NEW: Add a public string to hold the scene name ---
    [Tooltip("The exact name of your main game scene file")]
    public string mainGameSceneName = "YourGameSceneName"; // Replace with your actual scene name

    public void Play()
    {
        // --- MODIFIED: Pass the scene name instead of the index ---
        StartCoroutine(FadeOutAndLoadScene(mainGameSceneName));
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Player has Quit the Game");
    }

    // --- MODIFIED: The coroutine now accepts a string name ---
    private IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        float startTime = Time.time;
        Color startColor = fadeImage.color;
        fadeImage.gameObject.SetActive(true);

        while (Time.time < startTime + fadeDuration)
        {
            float t = (Time.time - startTime) / fadeDuration;
            fadeImage.color = new Color(startColor.r, startColor.g, startColor.b, t);
            yield return null;
        }

        fadeImage.color = new Color(startColor.r, startColor.g, startColor.b, 1f);

        // --- MODIFIED: Load the scene by its name ---
        SceneManager.LoadScene(sceneName);
    }
}