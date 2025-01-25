using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public InputSystem_Actions player_Actions;
    public PlayerAim aim { get; private set; } // Read Only

    private void Awake()
    {
        player_Actions = new InputSystem_Actions();
        aim = GetComponent<PlayerAim>();
    }

    void Start()
    {
    }

    private void OnEnable()
    {
        player_Actions.Enable();
    }

    private void OnDisable()
    {
        player_Actions.Disable();
    }
}
