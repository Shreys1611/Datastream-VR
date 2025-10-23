using UnityEngine;

/// <summary>
/// Handles general game utilities like cursor visibility and application quitting.
/// Attach this script to a persistent GameObject in your scene (e.g., MainControllerEmpty).
/// </summary>
public class GameUtilityManager : MonoBehaviour
{
    void Start()
    {
        // 1. Hide the mouse cursor and lock it to the center of the screen.
        // This is standard practice for first-person or immersive experiences.
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Debug.Log("Mouse cursor hidden and locked.");
    }

    void Update()
    {
        // 2. Check for the Escape key input every frame.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitApplication();
        }
    }

    /// <summary>
    /// Quits the application (build) or stops the editor playback (editor).
    /// </summary>
    public void QuitApplication()
    {
        Debug.Log("Escape key pressed. Quitting application...");

        // Quitting logic depends on the environment:
#if UNITY_EDITOR
        // If running in the Unity Editor, stop playing.
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // If running in a compiled build, exit the application.
            Application.Quit();
#endif
    }
}
