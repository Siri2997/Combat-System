using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerWeaponController : MonoBehaviour
{
    PlayerControl playerControl;
    [Header("Weapon Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform gunPoint;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float attackRange = 15f;
    [SerializeField] private float attackSpeed = 1f;

    [Header("Health settings")]
    [SerializeField] private Health_Bar healthBar;
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    private Transform currentTarget;
    private float nextAttackTime;
    private bool isAlive = true;

    private Animator animator;

    public bool IsAlive => isAlive;


    private void Start()
    {
        playerControl = GetComponent<PlayerControl>();
        currentHealth = maxHealth;
        healthBar.UpdateHealthBar(maxHealth, currentHealth);
        playerControl.playerControls.Player.Attack.performed += Context => Shoot();
        GameManager.Instance.RegisterCharacter(this.gameObject.transform);
        animator = GetComponentInChildren<Animator>();

    }

    private void Update()
    {
        if (!isAlive) return;

        if (currentTarget == null || !currentTarget.GetComponent<PlayerWeaponController>().IsAlive)
        {
            SelectRandomTarget();
        }

        if (currentTarget != null)
        {
            AimAtTarget();

            if (Time.time >= nextAttackTime)
            {
                //Shoot();
                nextAttackTime = Time.time + attackSpeed;
            }
        }
    }


    void Shoot()
    {
        if (Vector3.Distance(transform.position, currentTarget.position) <= attackRange)
        {
            // Get a bullet from the pool
            GameObject bullet = ObjectPool.Instance.GetBullet();
            bullet.transform.position = gunPoint.position;

            // Calculate direction to target
            Vector3 direction = (currentTarget.position - gunPoint.position).normalized;

            // Adjust the bullet's rotation to face the target
            bullet.transform.rotation = Quaternion.LookRotation(direction);

            // Apply force to the bullet
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.useGravity = false; // Disable gravity if not needed
            rb.AddForce(direction * bulletSpeed, ForceMode.VelocityChange);

            //Debug.Log($"Bullet Force Applied. Direction: {direction}, Speed: {bulletSpeed}");
            

            // Trigger the firing animation
            if (animator != null)
            {
                animator.SetTrigger("Fire");
            }
        }

    }


    private void AimAtTarget()
    {
        Vector3 direction = (currentTarget.position - transform.position).normalized;
        transform.forward = new Vector3(direction.x, 0, direction.z);
    }

    private void SelectRandomTarget()
    {
        var aliveCharacters = GameManager.Instance.GetAliveCharacters();
        if (aliveCharacters.Count > 1)
        {
            aliveCharacters.Remove(this.gameObject.transform); // Prevent targeting itself
            currentTarget = aliveCharacters[Random.Range(0, aliveCharacters.Count)].transform;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.UpdateHealthBar(maxHealth, currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isAlive = false;
        GameManager.Instance.DeregisterCharacter(this.gameObject.transform);
        gameObject.SetActive(false); // Disable the character
    }

}
