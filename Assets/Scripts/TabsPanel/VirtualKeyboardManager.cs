// VirtualKeyboardManager.cs
using UnityEngine;
using TMPro;

public class VirtualKeyboardManager : MonoBehaviour
{
    public static VirtualKeyboardManager Instance { get; private set; }

    [Header("References")]
    public GameObject keyboardObject;
    public TMP_InputField activeInputField { get; private set; }

    // ... (Awake and Start are the same)
    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this.gameObject); }
        else { Instance = this; }
    }
    void Start()
    {
        if (keyboardObject != null) { keyboardObject.SetActive(false); }
    }

    // ... (OpenKeyboard and CloseKeyboard are the same)
    public void OpenKeyboard(TMP_InputField inputField)
    {
        if (activeInputField != null && activeInputField != inputField)
        {
            activeInputField.readOnly = false;
        }
        activeInputField = inputField;
        if (activeInputField != null)
        {
            activeInputField.readOnly = true;
        }
        if (keyboardObject != null)
        {
            keyboardObject.SetActive(true);
        }
    }
    public void CloseKeyboard()
    {
        if (activeInputField != null)
        {
            activeInputField.readOnly = false;
        }
        activeInputField = null;
        if (keyboardObject != null)
        {
            keyboardObject.SetActive(false);
        }
    }

    // ... (TypeCharacter, Backspace, Clear are the same)
    public void TypeCharacter(string character)
    {
        if (activeInputField != null) { activeInputField.text += character; }
    }
    public void Backspace()
    {
        if (activeInputField != null && activeInputField.text.Length > 0)
        {
            activeInputField.text = activeInputField.text.Substring(0, activeInputField.text.Length - 1);
        }
    }
    public void Clear()
    {
        if (activeInputField != null) { activeInputField.text = ""; }
    }

    // --- NEW FUNCTIONS TO CONTROL THE CURSOR ---
    public void MoveCaretBackward()
    {
        if (activeInputField != null && activeInputField.caretPosition > 0)
        {
            activeInputField.caretPosition--;
        }
    }

    public void MoveCaretForward()
    {
        if (activeInputField != null && activeInputField.caretPosition < activeInputField.text.Length)
        {
            activeInputField.caretPosition++;
        }
    }

    public void MoveCaretToStart()
    {
        if (activeInputField != null)
        {
            activeInputField.caretPosition = 0;
        }
    }

    public void MoveCaretToEnd()
    {
        if (activeInputField != null)
        {
            activeInputField.caretPosition = activeInputField.text.Length;
        }
    }
}