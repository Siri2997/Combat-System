using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerControl playerControl;
    private InputSystem_Actions player_Actions;
    private PlayerInput playerInput;
    public Vector2 moveInput;

    [Header("Movement Info")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    private float currentSpeed;
    [SerializeField] private Vector3 movementDirection;
    [SerializeField] private bool isRunning;

    [Header("References")]
    [SerializeField] private CharacterController characterController;
    private Animator animator;

    [SerializeField] private float verticalVelocity;

    private void Start()
    {
        // Register the player with the GameManager
        GameManager.Instance.RegisterCharacter(transform);

        // Initialize components
        playerControl = GetComponent<PlayerControl>();
        playerInput = GetComponent<PlayerInput>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        currentSpeed = walkSpeed;

        AssignedInputEvents();
    }

    private void Update()
    {
        ApplyMovement();
        ApplyRotation();
        AnimatorControllers();
    }

    private void AssignedInputEvents()
    {
        // Access the player's unique input actions
        player_Actions = playerControl.playerControls;

        // Handle movement input
        player_Actions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        player_Actions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        // Handle running input
        player_Actions.Player.Run.performed += _ =>
        {
            currentSpeed = runSpeed;
            isRunning = true;
        };
        player_Actions.Player.Run.canceled += _ =>
        {
            currentSpeed = walkSpeed;
            isRunning = false;
        };
    }

    private void AnimatorControllers()
    {
        float xVelocity = Vector3.Dot(movementDirection.normalized, transform.right);
        float zVelocity = Vector3.Dot(movementDirection.normalized, transform.forward);

        animator.SetFloat("xVelocity", xVelocity, 0.1f, Time.deltaTime);
        animator.SetFloat("zVelocity", zVelocity, 0.1f, Time.deltaTime);

        bool playRunAnimation = isRunning && movementDirection.magnitude > 0;
        animator.SetBool("isRunning", playRunAnimation);
    }

    private void ApplyRotation()
    {
        if (moveInput.magnitude > 0)
        {
            // Rotate towards the movement direction
            Vector3 targetDirection = new Vector3(moveInput.x, 0, moveInput.y);
            transform.forward = targetDirection;
        }
    }

    private void ApplyMovement()
    {
        // Calculate movement direction based on input
        movementDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        // Apply movement with gravity
        ApplyGravity();
        characterController.Move(movementDirection * currentSpeed * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if (!characterController.isGrounded)
        {
            verticalVelocity -= 9.81f * Time.deltaTime;
        }
        else
        {
            verticalVelocity = -0.5f; // A small downward force to keep grounded
        }

        movementDirection.y = verticalVelocity;
    }
}
