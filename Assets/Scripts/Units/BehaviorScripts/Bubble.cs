using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : Unit
{
    // -- Serialize Fields --

    [Header("Unit Fields")]

    [SerializeField]
    float knockbackForce;

    [SerializeField]
    float knockbackRadius;

    [SerializeField]
    LayerMask layerMask;

    // -- Behavior --
    protected override void Start()
    {
        base.Start();
        KnockbackEnemies();
    }

    void KnockbackEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, knockbackRadius, layerMask);
        foreach (Collider enemy in colliders)
        {
            Vector3 explosionPoint = this.transform.position - Vector3.up;
            explosionPoint.y = 0;
            enemy.GetComponent<KinematicReset>().Knockback();
            enemy.GetComponent<Rigidbody>().AddExplosionForce(knockbackForce, explosionPoint, knockbackRadius, knockbackForce / 3);
        }
    }
}
