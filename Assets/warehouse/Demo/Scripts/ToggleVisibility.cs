using UnityEngine;

public class ToggleVisibility : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;

    // This function will be called when the button is clicked
    public void ToggleObject()
    {
        if (targetObject != null)
        {
            bool newState = !targetObject.activeSelf;
            targetObject.SetActive(newState);
        }
        else
        {
            Debug.LogWarning("No target object assigned to ToggleVisibility script.");
        }
    }
}
