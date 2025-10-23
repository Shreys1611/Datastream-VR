using UnityEngine;



public class VRMovementWithCollision : MonoBehaviour

{

    public float moveSpeed = 3f;

    private CharacterController characterController;

    private Transform cameraTransform;



    void Start()

    {

        characterController = GetComponent<CharacterController>();

        cameraTransform = GetComponentInChildren<Camera>().transform;

    }



    void Update()

    {

        // Check if teleport mode is active (Shift or Space pressed)

        bool teleportModeActive = Input.GetKey(KeyCode.LeftShift) ||

                Input.GetKey(KeyCode.RightShift) ||

                Input.GetKey(KeyCode.Space);



        // If teleport mode is active, don't process movement

        if (teleportModeActive)

        {

            // Still apply gravity so player doesn't float

            Vector3 gravityMovement = Vector3.zero;

            gravityMovement.y = Physics.gravity.y * Time.deltaTime;

            characterController.Move(gravityMovement);

            return; // Exit early, no movement processing

        }



        // Normal movement logic (only when teleport mode is NOT active)

        // Get WASD input

        float horizontal = Input.GetAxis("Horizontal"); // A/D

        float vertical = Input.GetAxis("Vertical");     // W/S

        float upDown = 0f;



        if (Input.GetKey(KeyCode.Q)) upDown = -1f;      // Down

        if (Input.GetKey(KeyCode.E)) upDown = 1f;       // Up



        // Create movement vector relative to camera direction

        Vector3 forward = cameraTransform.forward;

        Vector3 right = cameraTransform.right;



        // Remove vertical component for ground movement

        forward.y = 0f;

        right.y = 0f;

        forward.Normalize();

        right.Normalize();



        Vector3 movement = (forward * vertical + right * horizontal) * moveSpeed * Time.deltaTime;

        movement.y = upDown * moveSpeed * Time.deltaTime;



        // Apply gravity when not manually moving up/down

        if (upDown == 0f)

        {

            movement.y += Physics.gravity.y * Time.deltaTime;

        }



        // Move with collision detection

        characterController.Move(movement);

    }

}