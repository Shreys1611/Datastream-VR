using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(XRGrabInteractable))]
public class GrabbableBoxAnimator : MonoBehaviour
{
    [Header("UI & Animation")]
    // The World Space Canvas that holds the packet info
    [SerializeField] private Canvas packetInfoCanvas;
    // The animation clip for opening the box
    [SerializeField] private AnimationClip openAnimation;

    private Animator animator;
    private XRGrabInteractable grabInteractable;
    private bool isGrabbed = false;
    private bool isOpen = false;
    private Coroutine visibilityCoroutine;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Ensure the box starts closed and the canvas is hidden
        animator.SetBool("isOpen", false);
        if (packetInfoCanvas != null)
        {
            packetInfoCanvas.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
        grabInteractable.activated.AddListener(OnActivated);
    }

    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        grabInteractable.selectExited.RemoveListener(OnReleased);
        grabInteractable.activated.RemoveListener(OnActivated);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        isGrabbed = true;
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        isGrabbed = false;
    }

    private void OnActivated(ActivateEventArgs args)
    {
        if (isGrabbed)
        {
            ToggleBox();
        }
    }

    private void ToggleBox()
    {
        isOpen = !isOpen;
        animator.SetBool("isOpen", isOpen);

        if (visibilityCoroutine != null)
        {
            StopCoroutine(visibilityCoroutine);
        }

        if (isOpen)
        {
            // If the box is opening, start the coroutine to show the canvas AFTER the animation.
            visibilityCoroutine = StartCoroutine(ShowCanvasAfterAnimation());
        }
        else
        {
            // If the box is closing, hide the canvas immediately.
            HideCanvas();
        }
    }

    private IEnumerator ShowCanvasAfterAnimation()
    {
        // Wait for the exact length of the "open" animation clip.
        if (openAnimation != null)
        {
            yield return new WaitForSeconds(openAnimation.length);
        }

        // Now, enable the canvas.
        if (packetInfoCanvas != null)
        {
            packetInfoCanvas.gameObject.SetActive(true);
        }
    }

    private void HideCanvas()
    {
        if (packetInfoCanvas != null)
        {
            packetInfoCanvas.gameObject.SetActive(false);
        }
    }
}