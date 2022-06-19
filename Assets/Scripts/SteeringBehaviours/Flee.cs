using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Flee : ISteering
{
    private IArtificialMovement _self;
    private ITarget _target;

    public Flee(IArtificialMovement self)
    {
        _self = self;
        SetTarget(_self.Target);
    }

    public void SetTarget(ITarget target)
    {
        _target = target;
    }

    public Vector3 GetDir()
    {
        Vector3 dir = _self.transform.position - _target.transform.position;
        return dir;
    }

}

