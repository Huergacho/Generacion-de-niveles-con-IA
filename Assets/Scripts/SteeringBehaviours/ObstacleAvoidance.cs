using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class ObstacleAvoidance
{
    private IArtificialMovement _self;
    private ITarget _target;
    private ISteering _actualBehaviour;
    public ISteering ActualBehaviour => _actualBehaviour;
    public ObstacleAvoidance(IArtificialMovement self)
    {
        _self = self;
        _target = self.Target;
    }
    public void SetActualBehaviour(SteeringType desiredBehaviour)
    {
        _actualBehaviour = _self.Behaviours[desiredBehaviour];
    }
    public ISteering GetBehaviour(SteeringType behaviour)
    {
        if (!_self.Behaviours.ContainsKey(behaviour))
        {
            Debug.LogError("No existe ese Behaviour");
            return null;
        }

        return _self.Behaviours[behaviour];
    }

    public Vector3 GetDir()
    {

        Collider[] obj = Physics.OverlapSphere(_self.transform.position, _self.ObstacleAvoidanceSO.Radius, _self.ObstacleAvoidanceSO.ObstacleLayers);
        Collider closestObj = null;
        float nearDistance = 0;
        for (int i = 0; i < obj.Length; i++)
        {
            Collider currObs = obj[i];
            Vector3 dir = currObs.transform.position - _self.transform.position;
            float currentAngle = Vector3.Angle(_self.transform.forward, dir);
            if (currentAngle < _self.ObstacleAvoidanceSO.Angle / 2)
            {
                float currentDistance = Vector3.Distance(_self.transform.position, currObs.transform.position);
                if (closestObj == null || currentDistance < nearDistance)
                {
                    closestObj = currObs;
                    nearDistance = currentDistance;
                }
            }
        }
        if (closestObj != null)
        {
            if (nearDistance == _self.ObstacleAvoidanceSO.Radius)
            {
                nearDistance = _self.ObstacleAvoidanceSO.Radius - 0.00001f;
            }
            var point = closestObj.ClosestPoint(_self.transform.position);
            Vector3 dir = ((_self.transform.position + _self.transform.right * 0.0000000001f) - point);
            return dir.normalized;
        }

        return Vector3.zero;
    }
    public Vector3 GetFixedDir()
    {
        var direction = (GetDir() * _self.ObstacleAvoidanceSO.AvoidanceMult + _actualBehaviour.GetDir() * _self.ObstacleAvoidanceSO.BehaviourMult).normalized;
        return direction;
    }
    public Vector3 GetFixedDir(Vector3 dir)
    {
        var direction = (GetDir() * _self.ObstacleAvoidanceSO.AvoidanceMult + dir * _self.ObstacleAvoidanceSO.BehaviourMult).normalized;
        return direction;
    }

    public void SetTarget(ITarget target)
    {
        _target = target;
        _actualBehaviour.SetTarget(target);
    }
}
