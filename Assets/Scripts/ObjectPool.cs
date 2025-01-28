using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance; // Singleton instance for global access

    [Header("Pool Settings")]
    [SerializeField] private GameObject bulletPrefab; // Prefab to pool
    [SerializeField] private int poolSize = 20; // Initial size of the pool
    [SerializeField] private int maxPoolSize = 50; // Maximum number of bullets the pool can hold

    private Queue<GameObject> bulletPool;

    private void Awake()
    {
        // Ensure there's only one instance of ObjectPool
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        bulletPool = new Queue<GameObject>();
        CreateInitialPool();
    }

    /// <summary>
    /// Creates the initial pool of bullets.
    /// </summary>
    private void CreateInitialPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject newBullet = Instantiate(bulletPrefab);
            newBullet.SetActive(false); // Disable the bullet initially
            bulletPool.Enqueue(newBullet);
        }
    }

    /// <summary>
    /// Retrieves a bullet from the pool.
    /// </summary>
    /// <returns>A GameObject ready to use.</returns>
    public GameObject GetBullet()
    {
        if (bulletPool.Count > 0)
        {
            // Get the next available bullet from the pool
            GameObject bulletToGet = bulletPool.Dequeue();
            bulletToGet.SetActive(true); // Reactivate the bullet
            ResetBullet(bulletToGet); // Reset bullet's components (position, physics, etc.)
            return bulletToGet;
        }
        else
        {
            // If the pool is empty and hasn't reached the max size, create a new bullet
            if (bulletPool.Count < maxPoolSize)
            {
                Debug.LogWarning("Bullet pool is empty. Instantiating a new bullet.");
                GameObject newBullet = Instantiate(bulletPrefab);
                return newBullet;
            }
            else
            {
                // If the pool has reached its maximum size, return null or handle accordingly
                Debug.LogError("Bullet pool has reached its maximum size. Cannot instantiate more bullets.");
                return null;
            }
        }
    }

    /// <summary>
    /// Resets the bullet's components before returning to the pool.
    /// </summary>
    /// <param name="bullet">The bullet to reset.</param>
    private void ResetBullet(GameObject bullet)
    {
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Reset any velocity and angular velocity the bullet might have had
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // Set Rigidbody to kinematic if you're manually controlling the movement
            rb.isKinematic = false; // Ensure it's not kinematic when in the pool

            // Enable continuous collision detection for fast-moving bullets
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        // Reset bullet's transform if necessary
        bullet.transform.position = Vector3.zero;
        bullet.transform.rotation = Quaternion.identity;
    }

    /// <summary>
    /// Return a bullet to the pool.
    /// </summary>
    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false); // Disable the bullet
        bulletPool.Enqueue(bullet); // Add it back to the pool

        // Optionally reset the bullet when it’s returned to the pool
        ResetBullet(bullet);
    }
}
