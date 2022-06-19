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
    private bool _force;
    protected float _counter;

    public EnemyChaseState(IArtificialMovement self, INode root, SteeringType obsEnum)
    {
        _self = self;
        _root = root;
        _obsEnum = obsEnum;
    }

    public override void Awake()
    {
        _force = _self.HasTakenDamage; //we save if it was in chase because it took damage. 
        _counter = _self.IAStats.SteeringTime;
        _self.Avoidance.SetActualBehaviour(_obsEnum);
        _self.Avoidance.ActualBehaviour.SetTarget(_self.Target); //Lets set the player as target;
    }

    public override void Execute()
    {
        if (_force) //If it was chasing because of damage taken theeen...
            Timer();
        else if (!_self.IsTargetInSight() || _self.IsInShootingRange()) //if we didn´t take damage, player is not in sight or in shooting range then...  //TODO: add a max distance that can be seen and follow? MaxRangeDistance or somethign?
            _root.Execute();

        _self.LookDir(_self.Avoidance.GetSteeringDir());
        _self.Move(_self.transform.forward, _self.ActorStats.RunSpeed);
    }

    private void Timer()
    {
        _counter -= Time.deltaTime;
        if (_counter <= 0)
        {
            _counter = _self.IAStats.SteeringTime;
            _force = false;
        }
    }

    public override void Sleep()
    {
        _self.TakeHit(false); //We do a reset here for HasTakenDamge cuz he is already chasing it. 
        Debug.Log("After reset: " + _self.HasTakenDamage);
    }
}
