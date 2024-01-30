using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : UnitOld
{
    [SerializeField]
    GameObject projectilePrefab;
    public abstract GameObject FindTarget();

    public abstract void Aim();

    public abstract void Fire();
}
