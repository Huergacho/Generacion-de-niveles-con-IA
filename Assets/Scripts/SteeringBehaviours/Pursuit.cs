using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
class Pursuit : ISteering
{
    private Transform _self;
    private ITarget _target;
    private float _predictionTime;
    public Pursuit(ITarget target, Transform self, float predictionTime)
    {
        _self = self;
        _predictionTime = predictionTime;
        SetTarget(target);
    }

    public void SetTarget(ITarget target)
    {     
        _target = target;
    }

    public Vector3 GetDir()
    {
        var predictMult = _target.GetVel * _predictionTime;
       var dist = Vector3.Distance(_target.transform.position, _self.position) - 0.1f;
        if (predictMult >= dist)
        {
            predictMult = dist / 2;
        }
        Vector3 point = _target.transform.position + _target.GetFoward * Mathf.Clamp(predictMult,-dist, dist);

        Vector3 dir = (point - _self.position).normalized;
        return dir;
    }
}
