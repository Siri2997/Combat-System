using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    [Header("Enemy Settings")]
    [SerializeField] private float attackRange = 10f;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float attackSpeed = 1f;

    [SerializeField] private Transform gunPoint;
    [SerializeField] private GameObject bulletPrefab;

    private NavMeshAgent agent;
    private Transform currentTarget;
    private float nextAttackTime = 0f;

    [Header("Health Settings")]
    [SerializeField] private Health_Bar healthBar;
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    private float health;
    private Animator animator;

    private bool isAlive = true;

    public bool IsAlive => isAlive;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SelectRandomTarget();
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
                Shoot();
                nextAttackTime = Time.time + attackSpeed;
            }
        }
    }

    private void AimAtTarget()
    {
        Vector3 direction = (currentTarget.position - transform.position).normalized;
        transform.forward = new Vector3(direction.x, 0, direction.z);
    }


    private void Shoot()
    {
        if (Vector3.Distance(transform.position, currentTarget.position) <= attackRange)
        {
            GameObject bullet = ObjectPool.Instance.GetBullet();
            bullet.transform.position = gunPoint.position;
            bullet.transform.rotation = Quaternion.LookRotation(currentTarget.position - gunPoint.position);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce((currentTarget.position - gunPoint.position).normalized * bulletSpeed, ForceMode.VelocityChange);
            }
        }
    }


    private void SelectRandomTarget()
    {
        // Get a random target using GameManager
        Transform randomTarget = GameManager.Instance.GetRandomTarget();
        if (randomTarget != null)
        {
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
        gameObject.SetActive(false); // Disable the character
    }
    
}
