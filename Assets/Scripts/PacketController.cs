using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(Rigidbody))]
public class PacketController : MonoBehaviour
{
    [HideInInspector] public float progress = 0f; // normalized along belt
    [HideInInspector] public bool isOnBelt = false;
    [HideInInspector] public bool isHeld = false;
    [HideInInspector] public ConveyerBelt currentBelt = null;

    Rigidbody rb;
    XRGrabInteractable grabInteractable;


    // keep track of which belt triggers we are overlapping with
    readonly HashSet<ConveyerBelt> overlappingBelts = new HashSet<ConveyerBelt>();

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();


        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
        {
            Debug.LogWarning("Packet prefab missing XRGrabInteractable � adding one automatically.");
            grabInteractable = gameObject.AddComponent<XRGrabInteractable>();
        }


        // subscribe to grab events
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.selectExited.AddListener(OnSelectExited);


        // default physics config
        rb.useGravity = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
            grabInteractable.selectExited.RemoveListener(OnSelectExited);
        }
    }

    // Called when XR interactor grabs object
    void OnSelectEntered(SelectEnterEventArgs args)
    {
        isHeld = true;


        // detach from belt immediately so the belt stops driving its position
        if (currentBelt != null)
        {
            currentBelt.DetachPacket(this);
        }


        // ensure physics-driven while held (some setups expect non-kinematic while grabbed)
        rb.isKinematic = false;
    }

    void OnSelectExited(SelectExitEventArgs args)
    {
        isHeld = false;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        var pathFollower = GetComponent<PathFollower>();
        ConveyerBelt nearest = FindNearestOverlappingBelt();

        if (pathFollower != null && nearest != null)
        {
            // --- UPDATED SNAPPING LOGIC ---
            float nearestProgress = nearest.ComputeNearestProgress(transform.position);
            Vector3 snappedPosition = Vector3.Lerp(nearest.startPoint.position, nearest.endPoint.position, nearestProgress);
            transform.position = snappedPosition;

            // ADDED: Set the rotation to match the belt's direction
            Vector3 beltDirection = (nearest.endPoint.position - nearest.startPoint.position).normalized;
            if (beltDirection.sqrMagnitude > 0.01f)
            {
                transform.rotation = Quaternion.LookRotation(beltDirection);
            }

            // Then, resume the path.
            SetKinematic(true);
        }
        else if (pathFollower != null && nearest == null)
        {
            // CASE 2: The packet has a path BUT was dropped on the floor.
            // It loses its mission and becomes a normal object.
            Destroy(pathFollower); // Forget the path.
            SetKinematic(false);   // Obey physics.
        }
        else
        {
            // CASE 3: The packet has no path to begin with.
            // Use the original logic to attach to the nearest belt or fall.
            if (nearest != null)
            {
                nearest.AttachPacket(this);
            }
            else
            {
                currentBelt = null;
                isOnBelt = false;
                SetKinematic(false);
            }
        }
    }

    ConveyerBelt FindNearestOverlappingBelt()
    {
        ConveyerBelt best = null;
        float bestDist = float.MaxValue;


        foreach (var b in overlappingBelts)
        {
            if (b == null) continue;
            float t = b.ComputeNearestProgress(transform.position);
            Vector3 posOnBelt = Vector3.Lerp(b.startPoint.position, b.endPoint.position, t);
            float d = Vector3.Distance(transform.position, posOnBelt);
            if (d < bestDist)
            {
                bestDist = d;
                best = b;
            }
        }


        return best;
    }

    public void SetKinematic(bool k)
    {
        rb.isKinematic = k;
        if (k)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    // We use trigger enter/exit so packets know when they overlap a belt area.
    // The beltAreaTrigger on the ConveyorBelt must be a trigger collider that covers the belt region.
    void OnTriggerEnter(Collider other)
    {
        var belt = other.GetComponentInParent<ConveyerBelt>();
        if (belt != null)
            overlappingBelts.Add(belt);
    }


    void OnTriggerExit(Collider other)
    {
        var belt = other.GetComponentInParent<ConveyerBelt>();
        if (belt != null)
            overlappingBelts.Remove(belt);
    }
}
