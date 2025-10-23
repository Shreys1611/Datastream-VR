// InputFieldDetection.cs
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro; // Use TextMeshPro

// This script requires a TextMeshPro Input Field to be on the same GameObject
[RequireComponent(typeof(TMP_InputField))]
public class InputFieldDetection : MonoBehaviour, IPointerDownHandler // We only need to listen for the "down" click event
{
    private TMP_InputField myInputField;

    void Awake()
    {
        // Get a reference to the input field on this object
        myInputField = GetComponent<TMP_InputField>();
    }

    /// <summary>
    /// This function is automatically called by Unity's Event System when
    /// the user clicks on this UI element with a VR pointer.
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        // Tell our central manager to open the keyboard and set this field as the active target
        VirtualKeyboardManager.Instance.OpenKeyboard(myInputField);
    }
}