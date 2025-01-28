using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] private Transform currentTarget;
    private float nextAttackTime;
    private bool isAlive = true;

    private Animator animator;

    public bool IsAlive => isAlive;

    private void Start()
    {
        playerControl = GetComponent<PlayerControl>();

        // Initialize health variables
        currentHealth = maxHealth;
        healthBar.UpdateHealthBar(maxHealth, currentHealth);

        // Register player with GameManager
        GameManager.Instance.RegisterCharacter(this.gameObject.transform);

        animator = GetComponentInChildren<Animator>();

        // Bind shoot action
        playerControl.playerControls.Player.Attack.performed += context => Shoot();
    }

    private void Update()
    {
        if (!isAlive) return;

        // Get the current target from GameManager
        currentTarget = GameManager.Instance.GetTarget(transform);

        if (currentTarget == null || !currentTarget.GetComponent<PlayerWeaponController>().IsAlive)
        {
            SelectRandomTarget();
        }

        if (currentTarget != null)
        {
            AimAtTarget();

            if (Time.time >= nextAttackTime)
            {
                // Shoot automatically after time
                nextAttackTime = Time.time + attackSpeed;
            }
        }
    }

    void Shoot()
    {
        if (currentTarget == null) return; // No target

        if (Vector3.Distance(transform.position, currentTarget.position) <= attackRange)
        {
            // Get a bullet from the pool
            GameObject bullet = ObjectPool.Instance.GetBullet();
            bullet.transform.position = gunPoint.position;

            // Calculate direction to target
            Vector3 direction = (currentTarget.position - gunPoint.position).normalized;

            // Adjust the bullet's rotation to face the target
            bullet.transform.rotation = Quaternion.LookRotation(direction);
            Debug.DrawLine(gunPoint.position, currentTarget.position, Color.red, 2f);

            // Apply force to the bullet
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.AddForce(direction * bulletSpeed, ForceMode.VelocityChange);

            Debug.Log($"Bullet Force Applied. Direction: {direction}, Speed: {bulletSpeed}");

            // Trigger the firing animation
            if (animator != null)
            {
                animator.SetTrigger("Fire");
            }
        }
    }

    private void AimAtTarget()
    {
        if (currentTarget != null)
        {
            // Rotate towards the target
            Vector3 direction = (currentTarget.position - this.transform.position).normalized;
            transform.forward = new Vector3(direction.x, 0, direction.z);
        }
    }

    private void SelectRandomTarget()
    {
        // Get the list of alive characters
        List<Transform> alivePlayers = GameManager.Instance.GetAliveCharacters();

        if (alivePlayers.Count > 1) // Ensure there is more than one player alive
        {
            // Remove the current player from the list to avoid targeting oneself
            alivePlayers.Remove(this.transform);

            // Choose a random target from the remaining alive players
            Transform randomTarget = alivePlayers[Random.Range(0, alivePlayers.Count)];

            // Set the random target in GameManager
            GameManager.Instance.SetTarget(transform, randomTarget);
            currentTarget = randomTarget;
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
        healthBar.UpdateHealthBar(maxHealth, 0); // Update health bar to show deceased
        gameObject.SetActive(false); // Disable the character
    }
}
