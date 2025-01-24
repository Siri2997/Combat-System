using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
        float speed;
        float damage;
        Character target;

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
