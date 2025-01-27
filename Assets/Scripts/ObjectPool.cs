using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    public static ObjectPool Instance; // Singleton instance for global access

    [Header("Pool Settings")]
    [SerializeField] private GameObject bulletPrefab; // Prefab to pool
    [SerializeField] private int poolSize = 20; // Initial size of the pool

    private Queue<GameObject> bulletPool;

    private void Awake()
    {
        // Ensure there's only one instance of ObjectPool
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        bulletPool = new Queue<GameObject>();
        CreateIntialPool();
   
    }

    private void Start()
    {
            CreateIntialPool();
    }

    /// <summary>
    /// Creates a new object and adds it to the pool.
    /// </summary>
    /// <returns>The newly created object.</returns>
    private void CreateIntialPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject newBullet = Instantiate(bulletPrefab);
            newBullet.SetActive(false);
            bulletPool.Enqueue(newBullet);
        }
    }

    /// <summary>
    /// Retrieves an object from the pool.
    /// </summary>
    /// <returns>A GameObject ready to use.</returns>
    public GameObject GetBullet()
    {
        if (bulletPool.Count > 0)
        {
            GameObject bulletToGet = bulletPool.Dequeue();
            bulletToGet.SetActive(true); // Reactivate the object
            return bulletToGet;
        }
        else
        {
            // Create a new bullet if the pool is empty (optional)
            Debug.LogWarning("Bullet pool is empty. Instantiating a new bullet.");
            GameObject newBullet = Instantiate(bulletPrefab);
            return newBullet;
        }

    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }
    
}
