using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEvadeState<T> : State<T>
{
    private IArtificialMovement _self;
    private INode _root;
    private SteeringType _obsEnum;

    public EnemyEvadeState(IArtificialMovement self, INode root, SteeringType obsEnum)
    {
        _self = self;
        _root = root;
        _obsEnum = obsEnum;
    }

    public override void Awake()
    {
        _self.Avoidance.SetActualBehaviour(_obsEnum);
        _self.Avoidance.ActualBehaviour.SetTarget(_self.Target); //Lets set the player as target to evade;
    }

    public override void Execute()
    {
        if (!_self.HasTakenDamage && _self.IsEnemyFar()) //if we didn´t take damage AND player is not in sight or in shooting range then...  //TODO: add a max distance that can be seen and follow? MaxRangeDistance or somethign?
            _root.Execute();

        _self.LookDir(_self.Avoidance.GetSteeringDir());
        _self.Move(_self.transform.forward, _self.ActorStats.RunSpeed);
    }
}
