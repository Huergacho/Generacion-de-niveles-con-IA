using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityController : MonoBehaviour
{
    //EVENTS
    public event Action _onIdle;
    public event Action<Vector3, float> _onMove;
    public event Action _onShoot;

    protected abstract void InitFSM();

    protected virtual void Move(Vector3 dir, float desiredSpeed)
    {
        _onMove?.Invoke(dir, desiredSpeed);
    }

    protected virtual void Shoot()
    {
        _onShoot?.Invoke();
    }
}


