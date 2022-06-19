using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class ObstacleAvoidance
{
    private IArtificialMovement _self;
    private ISteering _actualBehaviour;

    public ISteering ActualBehaviour => _actualBehaviour;

    public ObstacleAvoidance(IArtificialMovement self)
    {
        _self = self;
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

    private Vector3 GetAvoidanceDir()
    {
        Collider[] obj = Physics.OverlapSphere(_self.transform.position, _self.IAStats.RangeAvoidance, _self.ActorStats.ObstacleLayers);
        Collider closestObj = null;
        float nearDistance = 0;
        for (int i = 0; i < obj.Length; i++)
        {
            Collider currObs = obj[i];
            Vector3 dir = currObs.transform.position - _self.transform.position;
            float currentAngle = Vector3.Angle(_self.transform.forward, dir);
            if (currentAngle < _self.IAStats.AngleAvoidance / 2)
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
            if (nearDistance == _self.IAStats.RangeAvoidance) //TODO: Facu is this even in use????
                nearDistance = _self.IAStats.RangeAvoidance - 0.00001f;

            var point = closestObj.ClosestPoint(_self.transform.position);
            Vector3 dir = ((_self.transform.position + _self.transform.right * 0.0000000001f) - point);
            return dir.normalized;
        }

        return Vector3.zero;
    }

    public Vector3 GetSteeringDir()
    {
        var direction = (GetAvoidanceDir() * _self.IAStats.AvoidanceWeight + _actualBehaviour.GetDir() * _self.IAStats.SteeringWeight).normalized;
        return direction;
    }

    public Vector3 GetDir(Vector3 dir)
    {
        var direction = (GetAvoidanceDir() * _self.IAStats.AvoidanceWeight + dir * _self.IAStats.SteeringWeight).normalized;
        return direction;
    }
}
