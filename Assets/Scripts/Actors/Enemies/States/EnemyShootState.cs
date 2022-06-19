using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class EnemyShootState<T> : State<T>
{
    private IArtificialMovement _ia;
    private INode _root;
    private SteeringType _obsEnum;

    public EnemyShootState(IArtificialMovement ia, INode root, SteeringType steering)
    {
        _ia = ia;
        _root = root;
        _obsEnum = steering;
    }

    public override void Awake()
    {
        _ia.Avoidance.SetActualBehaviour(_obsEnum);
        _ia.Avoidance.ActualBehaviour.SetTarget(_ia.Target); //Lets set the player as target;
        _ia.Move(Vector3.zero, 0);
    }

    public override void Execute()
    {
        if (!_ia.IsTargetInSight()|| !_ia.IsInShootingRange())
            _root.Execute();

        _ia.LookDir(_ia.Avoidance.GetSteeringDir());
        _ia.Move(Vector3.zero, 0);
        //_ia.Move(_ia.transform.forward, _ia.ActorStats.RunSpeed);
        _ia.Shoot();
    }
}

