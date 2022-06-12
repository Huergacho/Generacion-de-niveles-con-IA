using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Flee : ISteering
{
    private Transform _self;
    private ITarget _target;

    public Flee(ITarget target, Transform self)
    {
        _self = self;
        SetTarget(target);
    }

    public void SetTarget(ITarget target)
    {
        _target = target;
    }

    public Vector3 GetDir()
    {
        Vector3 dir = _self.position - _target.transform.position;
        return dir;
    }

}

