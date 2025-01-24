using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;
using UnityEngine.TextCore.Text;

public class Weapon : MonoBehaviour
{
    float attackSpeed; // Time between attacks
    float range;
    GameObject bulletPrefab;

    void Attack(Character target)
    {
        //if (target != null && Vector3.Distance(owner.Position, target.Position) <= range)
        //{
        //    // Spawn bullet
        //    GameObject bullet = Instantiate(bulletPrefab, owner.Position, Quaternion.identity);
        //    bullet.Initialize(target, damage);
        //}
    }
}
