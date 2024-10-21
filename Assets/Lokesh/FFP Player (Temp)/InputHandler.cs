using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    // Reference to the InputActionAsset containing all player input actions
    [SerializeField] private InputActionAsset playerControls;

    // Private InputAction variables to store specific player input actions
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction fireAction;
    private InputAction swapAction;

    // Public properties to expose player inputs for movement, looking, and jumping
    public Vector2 MoveInput { get; private set; }      // Stores movement input (e.g., WASD or left stick)
    public Vector2 LookInput { get; private set; }      // Stores camera look input (e.g., mouse or right stick)
    public bool JumpTriggred { get; private set; }      // Stores jump input (true when jump is triggered)
    public float sprintValue { get; private set; }
    public bool fireTriggred { get; private set; }
    public Vector2 swapInput { get; private set; }

    // Singleton instance to ensure only one InputHandler exists at a time
    public static InputHandler Instance { get; private set; }

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Implementing Singleton pattern to ensure a single instance of InputHandler
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make this object persist across scenes
        }
        else
            Destroy(gameObject); // Destroy any additional instances of InputHandler

        // Find and assign the specific input actions from the InputActionAsset
        moveAction = playerControls.FindActionMap("Player").FindAction("Move");
        lookAction = playerControls.FindActionMap("Player").FindAction("Look");
        jumpAction = playerControls.FindActionMap("Player").FindAction("Jump");
        sprintAction = playerControls.FindActionMap("Player").FindAction("Sprint");
        fireAction = playerControls.FindActionMap("Player").FindAction("Fire");
        swapAction = playerControls.FindActionMap("Player").FindAction("Swap");

        // Register event listeners for input actions
        RegisterInputAction();
    }

    // Method to register callbacks for the input actions
    private void RegisterInputAction()
    {
        // Register movement input: Capture the player's movement when the action is performed or canceled
        moveAction.performed += context => MoveInput = context.ReadValue<Vector2>(); // When movement input is performed, update MoveInput
        moveAction.canceled += context => MoveInput = Vector2.zero; // When movement input is canceled, reset MoveInput to zero

        // Register look input: Capture the player's look input (e.g., mouse or right stick movement)
        lookAction.performed += context => LookInput = context.ReadValue<Vector2>(); // When look input is performed, update LookInput
        lookAction.canceled += context => LookInput = Vector2.zero; // When look input is canceled, reset LookInput to zero

        // Register jump input: Capture when the jump button is pressed and released
        jumpAction.performed += context => JumpTriggred = true; // Set JumpTriggred to true when jump is pressed
        jumpAction.canceled += context => JumpTriggred = false; // Set JumpTriggred to false when jump is released

        sprintAction.performed += context => sprintValue = context.ReadValue<float>();
        sprintAction.canceled += context => sprintValue = 0f;

        fireAction.performed += context => fireTriggred = true;
        fireAction.canceled += context => fireTriggred = false;

        swapAction.performed += context => swapInput = context.ReadValue<Vector2>();
        swapAction.canceled += context => swapInput = Vector2.zero;
    }

    // Enable input actions when the object is enabled
    private void OnEnable()
    {
        moveAction.Enable();  // Enable movement input
        lookAction.Enable();  // Enable look input
        jumpAction.Enable();  // Enable jump input
        sprintAction.Enable();
        fireAction.Enable();
        swapAction.Enable();
    }

    // Disable input actions when the object is disabled
    private void OnDisable()
    {
        moveAction.Disable();  // Disable movement input
        lookAction.Disable();  // Disable look input
        jumpAction.Disable();  // Disable jump input
        sprintAction.Disable();
        fireAction.Disable();
        swapAction.Disable();
    }
}