using System;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;


public class Bullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject bulletImpactFx;

    [SerializeField] Rigidbody bulletRb => GetComponent<Rigidbody>();


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player") || collision.transform.CompareTag("Enemy"))
        {
            // Handle damage application on the target
            var healthComponent = collision.transform.GetComponent<PlayerWeaponController>();
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(UnityEngine.Random.Range(1f, 5f));
            }
        }

        CreateImapactFx(collision);
        ObjectPool.Instance.ReturnBullet(gameObject);
    }

    private void CreateImapactFx(Collision collision)
    {
        if (collision.contacts.Length > 0)
        {
            ContactPoint contact = collision.contacts[0];
            GameObject newImpactFx = Instantiate(bulletImpactFx, contact.point, Quaternion.LookRotation(contact.normal));
            Destroy(newImpactFx);
        }
    }
   
}
