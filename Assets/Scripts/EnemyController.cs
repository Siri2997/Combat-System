using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    [Header("Enemy Settings")]
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float attackRange = 10f;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private Transform gunPoint;
    [SerializeField] private GameObject bulletPrefab;

    private NavMeshAgent agent;
    private Transform currentTarget;
    private float nextAttackTime = 0f;

    [Header("Health")]
    [SerializeField] private Health_Bar health_Bar;
    [SerializeField] private float maxHealth = 3f;
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
        if (!isAlive || currentTarget == null) return;

        // Move towards the target
        agent.SetDestination(currentTarget.position);

        // Check attack range and attack
        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
        if (distanceToTarget <= attackRange && Time.time >= nextAttackTime)
        {
            Shoot();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    private void Shoot()
    {
        // Fire a bullet towards the target
        GameObject bullet = ObjectPool.Instance.GetBullet();
        bullet.transform.position = gunPoint.position;
        bullet.transform.rotation = Quaternion.LookRotation(currentTarget.position - gunPoint.position);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce((currentTarget.position - gunPoint.position).normalized * bulletSpeed, ForceMode.VelocityChange);
        }
    }

    private void SelectRandomTarget()
    {
        List<Transform> potentialTargets = GameManager.Instance.GetAliveCharacters();

        if (potentialTargets.Count > 0)
        {
            currentTarget = potentialTargets[Random.Range(0, potentialTargets.Count)];
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
                health_Bar.health_update_text.text = "Player Attacked : " + currentHealth;
            }
        }
    }

    private void Die()
    {
        isAlive = false;
        agent.isStopped = true;
        gameObject.SetActive(false); // Deactivate enemy
    }
    
}
