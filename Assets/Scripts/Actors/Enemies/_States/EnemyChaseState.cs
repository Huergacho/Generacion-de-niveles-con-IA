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
    protected float _counter;

    public EnemyChaseState(IArtificialMovement self, INode root, SteeringType obsEnum)
    {
        _self = self;
        _root = root;
        _obsEnum = obsEnum;
    }

    public override void Awake()
    {
        _self.OnCallToArms += CallToArms;
        _counter = _self.IAStats.SteeringTime;
        _self.Avoidance.SetActualBehaviour(_obsEnum);
        _self.Avoidance.ActualBehaviour.SetTarget(_self.Target); //Lets set the player as target;
    }

    public override void Execute()
    {
        if (!_self.HasTakenDamage && (!_self.IsTargetInSight() || _self.IsInShootingRange())) //if we didn´t take damage AND player is not in sight or in shooting range then...  //TODO: add a max distance that can be seen and follow? MaxRangeDistance or somethign?
                _root.Execute();

        if (_self.IsInShootingRange()) //If it was chasing because of damage taken and player is on shooting range!
            _root.Execute();

        _self.LookDir(_self.Avoidance.GetSteeringDir());
        _self.Move(_self.transform.forward, _self.ActorStats.RunSpeed);
    }

    private void CallToArms()
    {
        _self.TakeHit(true);
    }

    public override void Sleep()
    {
        _self.OnCallToArms -= CallToArms;
        _self.TakeHit(false); //We do a reset here for HasTakenDamge cuz he is already chasing it. 
    }
}
