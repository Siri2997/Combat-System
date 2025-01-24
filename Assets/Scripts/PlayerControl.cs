using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public InputSystem_Actions player_Actions;

    private void Awake()
    {
        player_Actions = new InputSystem_Actions();
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
