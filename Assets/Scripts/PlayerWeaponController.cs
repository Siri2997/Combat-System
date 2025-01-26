using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.UI;


public class PlayerWeaponController : MonoBehaviour
{
    PlayerControl playerControl;

    [Header("Weapon Settings")]

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed = 50f;
    [SerializeField] float REFERENCE_BULLET_SPEED = 50f;
    [SerializeField] float damageRate;
    [SerializeField] Transform gunPoint;

    [Header("Health")]
    [SerializeField] private Health_Bar health_Bar;
    [SerializeField] private GameObject deathEffect, hitEffect;
    [SerializeField] private float maxHealth =3f;
    private float currentHealth;
    private float health;

    private void Start()
    {
        playerControl = GetComponent<PlayerControl>();

        playerControl.player_Actions.Player.Attack.performed += Context => Shoot();

        //Health Variables
        currentHealth = maxHealth;
        health_Bar.UpdateHealthBar(maxHealth, currentHealth);   

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            //collision.gameObject.GetComponent<>
            Debug.Log("Damage Rate: " + damageRate);
        }

        if (collision.transform.CompareTag("Bullet"))
        {
            Invoke("PlayerHealthCheck", 1f);
        }
    }

    void Shoot()
    {

        // Get a bullet from the pool
        GameObject newBullet = ObjectPool.Instance.GetBullet();

        // Set bullet position and rotation
        newBullet.transform.position = gunPoint.position;
        newBullet.transform.rotation = Quaternion.LookRotation(gunPoint.forward);

        //// Fallback to instantiation if the pool is empty
        //newBullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));
        

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

    private void PlayerHealthCheck()
    {
        currentHealth -= Random.Range(0.5f, 1.5f);

        if (currentHealth <= 0)
        {
            //mention death Effect
        }

        else
        {
            //Mention Hit Effect
            Debug.Log("Shoot Called");
            health_Bar.UpdateHealthBar(maxHealth, currentHealth);
        }
    }
}
