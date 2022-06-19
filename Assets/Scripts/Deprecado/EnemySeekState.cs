using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class EnemySeekState<T> : State<T>
{
    private Action _onSeek; //mhhh no me facina
    private INode _root;
    private SteeringType _obsEnum;
    private IArtificialMovement _self;

    public EnemySeekState(IArtificialMovement self, INode root, SteeringType obsEnum, Action onSeek)
    {
        _self = self;
        _onSeek = onSeek;
        _root = root;
        _obsEnum = obsEnum;
    }

    public override void Awake()
    {
        _self.Avoidance.SetActualBehaviour(_obsEnum);
        _self.Avoidance.ActualBehaviour.SetTarget(_self.Target); //Lets set the player as target;
    }

    public override void Execute() //Si va a hacer algo, deberia hacer algo mas que lo que actualmente hace. 
    {
        _onSeek?.Invoke();
        _root.Execute();
    }
}
