using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
class Chase : ISteering
{
    private IArtificialMovement _self;
    private float _predictionTime;

    public Chase(IArtificialMovement self, float predictionTime)
    {
        _self = self;
        _predictionTime = predictionTime;
    }
    public void SetTarget(ITarget target)
    {
    }
    public Vector3 GetDir()
    {
        var predictMult = _self.Target.GetVel * _predictionTime;
        var dist = Vector3.Distance(_self.Target.transform.position, _self.transform.position) - 0.1f;
        if (predictMult >= dist)
        {
            predictMult = dist / 2;
        }
        Vector3 point = _self.Target.transform.position + _self.Target.GetFoward * Mathf.Clamp(predictMult,-dist, dist);

        Vector3 dir = (point - _self.transform.position).normalized;
        return dir;
    }
}
