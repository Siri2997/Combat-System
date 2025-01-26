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
