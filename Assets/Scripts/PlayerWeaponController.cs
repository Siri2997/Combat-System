using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.UI;


public class PlayerWeaponController : MonoBehaviour
{
    PlayerControl playerControl;

    [Header("Weapon Settings")]

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed = 50f;
    //[SerializeField] float REFERENCE_BULLET_SPEED = 50f;
    [SerializeField] float damageRate;
    [SerializeField] Transform gunPoint;

    [Header("Health")]
    [SerializeField] private Health_Bar health_Bar;
    [SerializeField] private GameObject deathEffect, hitEffect;
    [SerializeField] private float maxHealth = 3f;
    private float currentHealth;
    private float health;
    private Animator animator;

    //bool isHit;
    //bool isDead;

    private void Start()
    {
        playerControl = GetComponent<PlayerControl>();
        animator = GetComponentInChildren<Animator>();


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
            TakeDamage(Random.Range(0.5f, 1.5f)); // Apply random damage
            Debug.Log("Player hit by bullet.");
            //isHit = true;
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


        // Apply force to the bullet
        Rigidbody rb_newBullet = newBullet.GetComponent<Rigidbody>();
        if (rb_newBullet != null)
        {
            rb_newBullet.AddForce(gunPoint.forward * bulletSpeed, ForceMode.VelocityChange);
        }
        // Trigger the firing animation
        if (animator != null)
        {
            animator.SetTrigger("Fire");
        }


    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        health_Bar.UpdateHealthBar(maxHealth, currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Trigger hit animation
            if (animator != null)
            {
                animator.SetBool("isHit", true);
            }
        }
    }

    private void Die()
    {
        if (animator != null)
        {
            animator.SetBool("isDead", true);
        }

        Instantiate(deathEffect, transform.position, Quaternion.identity);
        gameObject.SetActive(false); // Deactivate the player
    }

}
