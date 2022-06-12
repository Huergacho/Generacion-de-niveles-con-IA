using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class EnemyChaseState<T> : State<T>
{
    private IArtificialMovement _self;
    private Func<bool> _isOnSight;
    private INode _root;
    private SteeringType _obsEnum;
    private Func<bool> _canShoot;

    public EnemyChaseState(IArtificialMovement self, Func<bool> canShoot, INode root, SteeringType obsEnum, Func<bool> isOnSight)
    {
        _self = self;
        _root = root;
        _obsEnum = obsEnum;
        _isOnSight = isOnSight;
        _canShoot = canShoot;
    }

    public override void Awake()
    {
        _self.Avoidance.SetActualBehaviour(_obsEnum);
    }

    public override void Execute()
    {
        if (!_isOnSight() || (_isOnSight() && _canShoot()))
        {
            _root.Execute();
            return;
        }

        _self.Move(_self.transform.forward, _self.ActorStats.RunSpeed);
        _self.SmoothRotation(_self.Avoidance.GetFixedDir());
    }

}
