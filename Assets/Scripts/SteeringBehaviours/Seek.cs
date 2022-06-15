using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : ISteering
{
    private IArtificialMovement _self;

    public Seek(IArtificialMovement self)
    {
        _self = self;
    }

    public Vector3 GetDir()
    {
        if(_self.Target == null)
        {
            return Vector3.zero;
        }
        Vector3 dir = _self.Target.transform.position - _self.transform.position;
        return dir.normalized;
    }

    public void SetTarget(ITarget newTarget)
    {
    }
}
