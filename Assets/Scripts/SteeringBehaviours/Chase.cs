using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
class Chase : ISteering
{
    private IArtificialMovement _self;
    private ITarget _target;

    public Chase(IArtificialMovement self)
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
        var predictMult = _target.GetVel * _self.IAStats.PredictionTime;
        var dist = Vector3.Distance(_target.transform.position, _self.transform.position) - 0.1f;
        if (predictMult >= dist)
            predictMult = dist / 2;

        Vector3 point = _target.transform.position + _target.GetFoward * Mathf.Clamp(predictMult,-dist, dist);

        Vector3 dir = (point - _self.transform.position).normalized;
        return dir;
    }
}
