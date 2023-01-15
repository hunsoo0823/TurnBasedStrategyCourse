using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAcition : MonoBehaviour
{
    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();

}
