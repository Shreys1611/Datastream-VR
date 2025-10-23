using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CapsuleCollider))]
public class ColliderFollowHead : MonoBehaviour
{
    public Transform xrCamera;
    private CapsuleCollider capsule;
    private CharacterController characterController;

    void Start()
    {
        capsule = GetComponent<CapsuleCollider>();
        characterController = GetComponent<CharacterController>();
        // Make sure the capsule is only used for triggers, not blocking movement
        capsule.isTrigger = true;
    }

    void Update()
    {
        if (xrCamera != null && capsule != null)
        {
            Vector3 localHead = transform.InverseTransformPoint(xrCamera.position);
            capsule.center = new Vector3(localHead.x, capsule.height / 2f, localHead.z);
        }
        if (xrCamera != null && characterController != null)
        {
            // Convert head position into local space
            Vector3 localHead = transform.InverseTransformPoint(xrCamera.position);

            // Update capsule height based on head Y (clamped for crouch/standing)
            float headHeight = Mathf.Clamp(localHead.y, 1.0f, 2.0f);
            characterController.height = headHeight;

            // Center the capsule under the head
            Vector3 newCenter = Vector3.zero;
            newCenter.x = localHead.x;
            newCenter.y = headHeight / 2f;
            newCenter.z = localHead.z;

            characterController.center = newCenter;
        }
    }
}
