using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    [Tooltip("Scene name to load. If empty, sceneIndex will be used.")]
    public string sceneName;
    public int sceneIndex = 1;

    [Header("Door Animation Settings")]
    public Animator doorAnimator;
    public float sceneLoadDelay = 2f; // how long to wait for doors to finish opening

    private bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        Debug.Log("Entered Trigger: " + other.name);
        if (other.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(PlayDoorAndFade());
        }
    }
    private System.Collections.IEnumerator PlayDoorAndFade()
    {
        // 1. Play door animation
        if (doorAnimator != null)
        {
            Debug.Log("Triggering door open animation...");
            doorAnimator.SetTrigger("Open");
        }
        yield return new WaitForSeconds(sceneLoadDelay);

        // 2. Fade + Load scene (using your SceneFader)
        if (!string.IsNullOrEmpty(sceneName))
        {
            Debug.Log("Player detected, fading to scene: " + sceneName);
            SceneFader.Instance.LoadSceneByName(sceneName);
        }
        else
        {
            Debug.Log("Player detected, fading to scene index: " + sceneIndex);
            SceneFader.Instance.LoadSceneByIndex(sceneIndex);
        }
        
    }
}
