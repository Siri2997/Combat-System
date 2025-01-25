using System;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject bulletImpactFx;

    [SerializeField]   Rigidbody bulletRb => GetComponent<Rigidbody>();
        float speed;
        float damage;
        Character target;

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

    void MoveTowardsTarget()
        {
            if (target != null)
            {
            //Position = Vector3.MoveTowards(this.transform.position, target.tr, speed * Time.deltaTime);
            //    if (Vector3.Distance(Position, target.Position) < threshold)
            //    {
            //        target.TakeDamage(damage);
            //        Destroy(this); // Remove bullet from the scene
            //    }
            }
        }
}
