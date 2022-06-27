using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
class EnemyIdleStates<T> : State<T>
{
    private IArtificialMovement _self;
    private Func<bool> _outOfIdleCommand;
    private Action _onIdle;
    private INode _root;
    public EnemyIdleStates(IArtificialMovement self, Action onIdle, INode root,Func<bool> outOfIdleCommand)
    {
        _self = self;
        _onIdle = onIdle;
        _root = root;
        _outOfIdleCommand = outOfIdleCommand;
    }

    public override void Awake()
    {
        _self.LifeController.OnTakeDamage += TakeHit;
    }

    public override void Execute() //TODO: FACU check line of sight? check if receives damage? something
    {
        if (_outOfIdleCommand())
            _root.Execute();

        _onIdle?.Invoke();
    }

    private void TakeHit()
    {
        _self.TakeHit(true);
        _root.Execute();
    }

    public override void Sleep()
    {
        _self.LifeController.OnTakeDamage -= TakeHit;
    }
}
