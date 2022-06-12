using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    private int _lastFrame;
    private bool cache;
    private EntityModel _self;

    public void Awake()
    {
        _self = GetComponent<EntityModel>();
    }

    public bool CanSeeTarget(Transform target)
    {
        if (_lastFrame == Time.frameCount) return cache; //retorno el cache

        _lastFrame = Time.frameCount;
        cache = false;

        Vector3 diff = target.position - _self.transform.position;
        float distance = diff.magnitude;
        if (distance > _self.ActorStats.RangeVision)
            return false;

        var angleToTarget = Vector3.Angle(diff, _self.transform.forward);
        if (angleToTarget > _self.ActorStats.AngleVision / 2)
            return false;

        if (Physics.Raycast(_self.transform.position, diff.normalized, distance, _self.ActorStats.ObstacleLayers))
            return false;

        cache = true;
        return true;
    }

    public List<Transform> CheckTargets()
    {
        Collider[] colls = Physics.OverlapSphere(_self.transform.position, _self.ActorStats.RangeVision, _self.ActorStats.TargetLayer);
        List<Transform> targets = new List<Transform>();

        for (int i = 0; i < colls.Length; i++)
        {
            if (CanSeeTarget(colls[i].transform))
                targets.Add(colls[i].transform);
        }

        return targets;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _self.ActorStats.RangeVision);

        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, _self.ActorStats.AngleVision / 2, 0) * transform.forward * _self.ActorStats.RangeVision);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -_self.ActorStats.AngleVision / 2, 0) * transform.forward * _self.ActorStats.RangeVision);
    }

    public bool CanSeeManyTargets()
    {
        if (CheckTargets().Count <= 0)
            return false;

        return true;
    }

    public bool CheckForOneTarget()
    {
        var target = GetTheFirstTarget();

        if (target == null) return false;
        if (!CanSeeTarget(target)) return false;

        return true;
    }

    public Transform GetTheFirstTarget()
    {
        if (CheckTargets().Count <= 0)
            return null;

        return CheckTargets()[0];
    }

    public Transform GetTheLastTarget()
    {
        if (CheckTargets().Count < 1)
            return GetTheFirstTarget();

        return CheckTargets()[CheckTargets().Count - 1];
    }
}
