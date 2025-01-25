using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    PlayerControl playerControl;
    InputSystem_Actions player_Actions;

    Vector2 lookInput;

    [Header("Aim Info")]
    [SerializeField] LayerMask aimLayerMask;
    Vector3 lookingDirection;
    [SerializeField] Transform aim;


    private void Start()
    {
        playerControl = GetComponent<PlayerControl>();
        AssignInputEvents();
    }

    private void Update()
    {
        aim.position = new Vector3(GetMousePosition().x, transform.position.y+1, GetMousePosition().z);

    }

    private void AssignInputEvents()
    {
        player_Actions = playerControl.player_Actions;

        player_Actions.Player.Look.performed += Context => lookInput = Context.ReadValue<Vector2>();
        player_Actions.Player.Look.canceled += Context => lookInput = Vector2.zero;
    }
    public Vector3 GetMousePosition()
    {
        //aim.position = new Vector3(hitInfo.point.x, transform.position.y, hitInfo.point.z);

        Ray ray= Camera.main.ScreenPointToRay(lookInput);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            return hitInfo.point;
        }
        return Vector3.zero;
    }
}
