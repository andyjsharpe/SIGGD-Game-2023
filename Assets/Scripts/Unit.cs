using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    //What will the unit do on spawn?
    protected abstract void Awake();

    //What will the unit do when activated? (Fire? Other?)
    protected abstract void Activate();

    //What will the unit do on end
    protected abstract void End();
}
