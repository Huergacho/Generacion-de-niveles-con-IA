using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class EnemyChaseState<T> : State<T>
{
    private IArtificialMovement _self;
    private INode _root;
    private SteeringType _obsEnum;

    public EnemyChaseState(IArtificialMovement self, INode root, SteeringType obsEnum)
    {
        _self = self;
        _root = root;
        _obsEnum = obsEnum;
    }

    public override void Awake()
    {
        _self.TakeHit(false); //We do a reset here for HasTakenDamge cuz he is already chasing it. 
        _self.Avoidance.SetActualBehaviour(_obsEnum);
    }

    public override void Execute()
    {
        if (!_self.IsTargetInSight() || _self.IsInShootingRange())
        {
            _root.Execute();
            return;
        }

        _self.Move(_self.transform.forward, _self.ActorStats.RunSpeed);
        _self.LookDir(_self.Avoidance.GetDir());
    }
}
