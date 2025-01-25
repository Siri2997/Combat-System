using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using UnityEditor.Rendering.LookDev;
using System;

public class PlayerMovement : MonoBehaviour
{
    private PlayerControl playerControl; 
    private InputSystem_Actions player_Actions;
    public Vector2 moveInput;
    
    public float aim_point;

    [SerializeField] CharacterController characterController;
    private Animator animator;

    [Header("Movement Info")]
    [SerializeField] Vector3 movementDirection;

    [SerializeField]private bool isRunning;

    [SerializeField]private float walkSpeed;
    [SerializeField]private float runSpeed;

    private float speed;

   

    [SerializeField] float verticalVelocity;


    private void Start()
    {
        playerControl = GetComponent<PlayerControl>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        speed = walkSpeed;

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
        player_Actions = playerControl.player_Actions;

        player_Actions.Player.Move.performed += Context => moveInput = Context.ReadValue<Vector2>();
        player_Actions.Player.Move.canceled += Context => moveInput = Vector2.zero;

        player_Actions.Player.Run.performed += Context =>
        {
            speed = runSpeed;
            isRunning = true;
        };

        player_Actions.Player.Run.canceled += Context =>
        {
            speed = walkSpeed;
            isRunning = false;
        };
    }

    private void AnimatorControllers()
    {
        float xVelocity = Vector3.Dot(movementDirection.normalized, transform.right);
        float zVelocity = Vector3.Dot(movementDirection.normalized, transform.forward);

        animator.SetFloat("xVelocity", xVelocity, .1f, Time.deltaTime);
        animator.SetFloat("zVelocity", zVelocity, .1f, Time.deltaTime);
        bool playRunAnimation = isRunning && movementDirection.magnitude>0 ;   
        animator.SetBool("isRunning", playRunAnimation);

    }

    private void ApplyRotation()
    {
       Vector3 lookingDirection = playerControl.aim.GetMousePosition() - transform.position;
       lookingDirection.y = 0f;
       lookingDirection.Normalize();
       transform.forward = lookingDirection;
    }

    private void ApplyMovement()
    {
        movementDirection = new Vector3(moveInput.x, 0, moveInput.y);

        ApplyGravity();
        if (movementDirection.magnitude > 0)
        {
            characterController.Move(movementDirection * Time.deltaTime * speed);
        }
    }

    private void ApplyGravity()
    {
        if (!characterController.isGrounded)
        {
            verticalVelocity = verticalVelocity - 9.81f * Time.deltaTime;
            movementDirection.y = verticalVelocity;
        }
        else
        {
            verticalVelocity = -0.5f;
        }
    }

}
