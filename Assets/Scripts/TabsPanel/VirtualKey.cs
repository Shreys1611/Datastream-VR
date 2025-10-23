// VirtualKey.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class VirtualKey : MonoBehaviour
{
    [Header("Key Value")]
    public string keyValue;

    [Header("Visuals")]
    public TextMeshProUGUI keyText;

    private Button keyButton;
    private bool isLowercase = true;

    void Awake()
    {
        keyButton = GetComponent<Button>();
        // Ensure the listener is set up correctly
        if (keyButton.onClick.GetPersistentEventCount() == 0)
        {
            keyButton.onClick.AddListener(OnKeyPress);
        }
    }

    void Update()
    {
        if (isLowercase != LetterCaseDetection.Lowercase)
        {
            isLowercase = LetterCaseDetection.Lowercase;
            UpdateKeyText();
        }
    }

    // --- THIS IS THE FINAL, ROBUST OnKeyPress FUNCTION ---
    public void OnKeyPress()
    {
        // This check is the most important part.
        if (VirtualKeyboardManager.Instance == null)
        {
            Debug.LogError("FATAL ERROR: VirtualKeyboardManager.Instance is NULL. Cannot process key press. Ensure the VirtualKeyboardManager script is on an active GameObject in the scene and is enabled.");
            return; // Stop here to prevent the NullReferenceException
        }

        Debug.Log($"Key Pressed: {keyValue}");

        switch (keyValue.ToLower())
        {
            case "delete":
                VirtualKeyboardManager.Instance.Backspace();
                break;
            case "clear":
                VirtualKeyboardManager.Instance.Clear();
                break;
            case "letter case":
                LetterCaseDetection.Lowercase = !LetterCaseDetection.Lowercase;
                break;
            case "<":
                VirtualKeyboardManager.Instance.MoveCaretBackward();
                break;
            case ">":
                VirtualKeyboardManager.Instance.MoveCaretForward();
                break;
            case "<<":
                VirtualKeyboardManager.Instance.MoveCaretToStart();
                break;
            case ">>":
                VirtualKeyboardManager.Instance.MoveCaretToEnd();
                break;
            default:
                VirtualKeyboardManager.Instance.TypeCharacter(keyText.text);
                break;
        }
    }

    private void UpdateKeyText()
    {
        if (keyText == null) return;
        if (keyValue.Length == 1 && char.IsLetter(keyValue[0]))
        {
            keyText.text = isLowercase ? keyValue.ToLower() : keyValue.ToUpper();
        }
        else
        {
            keyText.text = keyValue;
        }
    }
}