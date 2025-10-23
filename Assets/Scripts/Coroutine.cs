using UnityEngine;
using System.Collections; // Required for Coroutines

public class DelayedUI : MonoBehaviour
{
    // Drag your UI GameObject here in the Inspector
    public GameObject uiElement; 

    void Start()
    {
        // Start the coroutine when the game begins
        StartCoroutine(ShowUIAfterDelay(2f)); 
    }

    IEnumerator ShowUIAfterDelay(float delayTime)
    {
        // Wait for the specified number of seconds
        yield return new WaitForSeconds(delayTime); 
        
        // After the wait, activate the UI element
        uiElement.SetActive(true);
    }
}