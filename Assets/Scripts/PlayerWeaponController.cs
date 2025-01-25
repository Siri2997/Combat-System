using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    PlayerControl playerControl;

    [Header("Weapon Settings")]

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed = 50f;
    [SerializeField] float REFERENCE_BULLET_SPEED = 50f;
    [SerializeField] Transform gunPoint;

    private void Start()
    {
        playerControl = GetComponent<PlayerControl>();

        playerControl.player_Actions.Player.Attack.performed += Context => Shoot();

    }

    void Shoot()
    {
        // Check if the bullet prefab and gun point are set
        if (bulletPrefab == null || gunPoint == null)
        {
            Debug.LogWarning("Bullet prefab or gun point is not assigned!");
            return;
        }

        // Get a bullet from the pool
        GameObject newBullet = ObjectPool.Instance.GetBullet();

        // Set bullet position and rotation
        newBullet.transform.position = gunPoint.position;
        newBullet.transform.rotation = Quaternion.LookRotation(gunPoint.forward);
       // Fallback to instantiation if the pool is empty
         newBullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));
  

        //ObjectPool.instance.SaySomething();

        // Set bullet velocity
        Rigidbody rb_newBullet = newBullet.GetComponent<Rigidbody>();
        if (rb_newBullet != null)
        {
            rb_newBullet.mass = REFERENCE_BULLET_SPEED / bulletSpeed;
        }

       

        // Trigger the firing animation
        Animator animator = GetComponentInChildren<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Fire");
        }


    }
}
