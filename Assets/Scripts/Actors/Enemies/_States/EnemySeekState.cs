using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class EnemySeekState<T> : State<T>
{
    private INode _root;
    private SteeringType _obsEnum;
    private IArtificialMovement _self;
    private IThief _thief;
    private Vector3 currentTarget;

    public EnemySeekState(IArtificialMovement self, INode root, SteeringType obsEnum)
    {
        _self = self;
        _root = root;
        _obsEnum = obsEnum;


        if (_self is IThief)
        {
            _thief = _self as IThief;
        }
    }

    public override void Awake()
    {
        _self.Avoidance.SetActualBehaviour(_obsEnum);
        _thief.GetATarget();
        currentTarget = _self.Destination;
        //_self.Avoidance.ActualBehaviour.SetTarget(_self.Target);
    }

    public override void Execute() //Si va a hacer algo, deberia hacer algo mas que lo que actualmente hace. 
    {
        if (_self.HasTakenDamage || _self.IsTargetInSight() || _thief?.ItemStolen != null || _thief?.NextTarget == null) //if we didn´t take damage AND player is not in sight or in shooting range then... 
            _root.Execute();

        if (CheckIfNearDestination())
            _root.Execute();

        _self.LookDir(_self.Avoidance.GetDir(currentTarget));
        _self.Move(_self.transform.forward, _self.ActorStats.RunSpeed);
    }

    private bool CheckIfNearDestination()
    {
        currentTarget.y = _self.transform.position.y;
        Vector3 diff = currentTarget - _self.transform.position;
        Vector3 dir = diff.normalized;

        float distance = diff.magnitude;
        return (distance < _self.IAStats.NearTargetRange);
    }
}
