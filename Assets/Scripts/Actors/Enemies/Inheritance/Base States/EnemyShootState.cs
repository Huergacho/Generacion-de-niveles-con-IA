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
    private Func<bool> _onDetect;
    private Func<bool> _onCheckDistance;

    public EnemyShootState(IArtificialMovement ia, INode root, SteeringType steering, Func<bool> onDetect, Func<bool> onCheckDistance)
    {
        _ia = ia;
        _root = root;
        _obsEnum = steering;
        _onDetect = onDetect;
        _onCheckDistance = onCheckDistance;
    }

    public override void Awake()
    {
        _ia.Avoidance.SetActualBehaviour(_obsEnum);
    }

    public override void Execute()
    {
        if (!_onDetect() || !_onCheckDistance())
        {
            _root.Execute();
            return;
        }

        _ia.Move(_ia.transform.forward, _ia.ActorStats.RunSpeed);
        _ia.SmoothRotation(_ia.Avoidance.GetFixedDir());
        _ia.Shoot();
    }
}

