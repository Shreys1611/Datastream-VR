using UnityEngine;
using UnityEngine.UI;

public class ToggleAnimationController : MonoBehaviour
{
    public Toggle toggle; // Reference to your Toggle
    public Animator animator; // Animator with loopable animation (optional)
    public string loopStateName = "LoopAnim"; // Name of looping animation
    public string idleStateName = "Idle"; // Name of idle/stop state

    [Header("Optional UI")]
    public Text labelText; // Drag your UI Text here

    [Header("Target Conveyor")]
    public ConveyerBelt targetBelt; // Assign in Inspector

    private void Start()
    {
        if (toggle == null)
            toggle = GetComponent<Toggle>();


        if (toggle != null)
        {
            toggle.onValueChanged.AddListener(OnToggleChanged);

            // Force initialize with current toggle state
            OnToggleChanged(toggle.isOn);
        }
    }

    private void OnToggleChanged(bool isOn)
    {
        // Update animator if present
        if (animator != null)
        {
            if (isOn)
                animator.Play(loopStateName);
            else
                animator.Play(idleStateName);
        }

        // Update ONLY the assigned conveyor belt
        if (targetBelt != null)
            targetBelt.SetRunning(isOn);

        // Update UI text
        if (labelText != null)
            labelText.text = isOn ? "On" : "Off";
    }
}
