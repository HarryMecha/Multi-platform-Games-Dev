using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    // Serialized fields for player movement settings
    [Header("Movement")]
    [SerializeField] private float walkSpeed;         // Walking speed of the player
    [SerializeField] private float sprintMultiplier;

    // Serialized fields for jump mechanics
    [Header("Jumping")]
    [SerializeField] private float jumpForce;         // Force applied when jumping
    [SerializeField] private float gravity;           // Gravity force applied when falling

    // Serialized fields for mouse sensitivity and camera controls
    [Header("Mouse Sensitivity")]
    [SerializeField] private float mouseSensitivity;  // Sensitivity of mouse for looking around
    [SerializeField] private float upDownRange;       // Vertical rotation limit to avoid looking too far up or down
    [SerializeField] private bool invertYAxis;        // Option to invert Y-axis of mouse (not used in this script, but can be added)

    // Internal state variables
    private Vector3 currentMovement = Vector3.zero;   // Stores current movement (x, y, z) of the player
    private float verticalRotation;                   // Rotation for looking up and down
    private bool canJump = true;                      // Whether the player is allowed to jump (prevents mid-air jumps)

    // References to external components
    private InputHandler inputHandler;                // Reference to the InputHandler script for capturing player input
    private CharacterController characterController;  // Reference to the CharacterController component
    private Camera mainCamera;                        // Reference to the main camera for handling rotation

    // Called when the script instance is being loaded
    private void Awake()
    {
        // Get references to other necessary components
        inputHandler = InputHandler.Instance;                         // Get the singleton instance of InputHandler
        characterController = GetComponent<CharacterController>();    // Get the CharacterController component attached to this GameObject
        mainCamera = Camera.main;                                     // Get the main camera (usually attached to the player)
    }

    // Called every frame
    void Update()
    {
        HandleMovement();  // Handle player movement input
        HandleRotation();  // Handle player camera rotation input
    }

    // Handles the player's camera and character rotation based on mouse movement
    private void HandleRotation()
    {
        // Horizontal rotation (left-right) based on the X-axis input from the mouse
        float mouseXRotation = inputHandler.LookInput.x * mouseSensitivity;
        transform.Rotate(0, mouseXRotation, 0); // Rotate the player horizontally (y-axis) based on mouse input

        // Vertical rotation (up-down) based on the Y-axis input from the mouse
        verticalRotation -= inputHandler.LookInput.y * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange); // Clamp the vertical rotation to prevent looking too far up or down

        // Apply the vertical rotation to the camera
        mainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    // Handles player movement input including walking and jumping
    private void HandleMovement()
    {
        // Get the movement input from the InputHandler (in the form of a 2D vector: x for left-right, y for forward-backward)
        Vector3 inputDirection = new Vector3(inputHandler.MoveInput.x, 0f, inputHandler.MoveInput.y);

        // Convert local movement input to world direction based on the player's current orientation
        Vector3 worldDirection = transform.forward * inputDirection.z + transform.right * inputDirection.x;
        worldDirection.Normalize(); // Normalize to maintain consistent speed

        // Set the horizontal movement (X and Z axes) based on the calculated direction and walk speed
        currentMovement.x = worldDirection.x * walkSpeed;
        currentMovement.z = worldDirection.z * walkSpeed;

        // Handle jumping and gravity
        HandleJump();

        // Apply the final movement vector to the CharacterController
        characterController.Move(currentMovement * Time.deltaTime);
    }

    // Handles the jump mechanics and gravity
    private void HandleJump()
    {
        // Reset jump flag if the player is not currently triggering the jump action
        if (!inputHandler.JumpTriggred)
            canJump = true;

        // Check if the character is grounded (on the ground)
        if (characterController.isGrounded)
        {
            // Apply a small downward force to ensure the player stays grounded
            currentMovement.y = -0.5f;

            // Check if the player has triggered a jump and is allowed to jump (not already jumping)
            if (inputHandler.JumpTriggred && canJump)
            {
                currentMovement.y = jumpForce; // Apply upward force (jump)
                canJump = false;               // Prevent multiple jumps until grounded again
            }
        }
        // If the player is not grounded (in the air), apply gravity
        else
            currentMovement.y -= gravity * Time.deltaTime; // Apply gravity over time
    }
}