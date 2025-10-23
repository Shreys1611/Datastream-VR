using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskButtonManager : MonoBehaviour
{
    public Button[] taskButtons;
    public Color initialColor = Color.red;
    public Color completedColor = Color.green;

    private ControlPanelManager controlPanelManager; // This variable is correct
    private TaskData[] tasks;

    // --- THIS IS THE CORRECTED LINE ---
    public void InitializeButtons(TaskData[] taskArray, ControlPanelManager manager)
    {
        tasks = taskArray;
        controlPanelManager = manager; // Now this assignment works correctly

        if (tasks.Length != taskButtons.Length)
        {
            Debug.LogError("TaskData array size does not match the number of Task Buttons!");
            return;
        }

        for (int i = 0; i < taskButtons.Length; i++)
        {
            int index = i;
            taskButtons[i].onClick.RemoveAllListeners();
            // This line now works because controlPanelManager is the correct type
            taskButtons[i].onClick.AddListener(() => controlPanelManager.SelectTask(index));
            taskButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = tasks[i].taskName;
        }

        UpdateButtonColors();
    }

    public void UpdateButtonColors()
    {
        for (int i = 0; i < tasks.Length; i++)
        {
            Image buttonImage = taskButtons[i].GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = tasks[i].isCompleted ? completedColor : initialColor;
            }
        }
    }
}