using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public InputSystem_Actions playerControls;

    private void Awake()
    {
        playerControls = new InputSystem_Actions(); // Assuming you're using this asset
    }

    private void OnEnable()
    {
        playerControls.Enable(); // Enable the input actions
    }

    private void OnDisable()
    {
        playerControls.Disable(); // Disable the input actions
    }
}
