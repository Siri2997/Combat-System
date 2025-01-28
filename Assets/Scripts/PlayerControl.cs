using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] public InputSystem_Actions playerControls { get; private set; } // Per-player input
    //public PlayerInput playerInput;

    private void Awake()
    {
        //playerInput = GetComponent<PlayerInput>(); // Get the PlayerInput component
        playerControls = new InputSystem_Actions();

        // Enable only this player's input
        //playerInput.actions.Enable();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}
