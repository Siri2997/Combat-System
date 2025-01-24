using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    PlayerControl playerControl;
    Animator anim;

    private void Start()
    {
        playerControl = GetComponent<PlayerControl>();
        anim = GetComponentInChildren<Animator>();

        playerControl.player_Actions.Player.Attack.performed += Context => Shoot();

    }

    void Shoot()
    {
        anim.SetTrigger("Fire");
    }
}
