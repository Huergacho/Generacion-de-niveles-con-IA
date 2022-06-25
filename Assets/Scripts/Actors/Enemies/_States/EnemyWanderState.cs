using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWanderState<T> : State<T>
{
    private IThief _self;
    private IArtificialMovement _ia;
    private INode _root;
    private SteeringType _obsEnum;

    private float _randomAngle;
    private Vector3 _dir;

    public EnemyWanderState(IThief self, INode root, SteeringType obsEnum, float randomAngle)
    {
        _self = self;
        _root = root;
        _obsEnum = obsEnum;
        _ia = (_self as IArtificialMovement);
        _randomAngle = randomAngle;
    }

    public override void Awake()
    {
        _ia.LifeController.OnTakeDamage += TakeHit;
        _self.Avoidance.SetActualBehaviour(_obsEnum);
        //_self.Avoidance.ActualBehaviour.SetTarget(_self.Target);

        _dir = Quaternion.Euler(0, Random.Range(-_randomAngle, _randomAngle), 0) * _self.transform.forward;
    }
    public override void Execute()
    {
        if (_ia.IsTargetInSight()) //if we took damage or we saw the player....
            _root.Execute();

        _self.LookDir(_self.Avoidance.GetDir(_dir));
        _self.Move(_self.transform.forward, _self.ActorStats.RunSpeed);
    }

    private void TakeHit()
    {
        _ia.TakeHit(true);
        _root.Execute();
    }

    public override void Sleep()
    {
        _ia.LifeController.OnTakeDamage -= TakeHit;
    }
}
