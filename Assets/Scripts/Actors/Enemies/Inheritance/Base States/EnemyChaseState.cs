using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class EnemyChaseState<T> : State<T>
{
    private Action<Vector3,float> _onChase;
    private Func<bool> _isOnSight;
    private INode _root;
    private ObstacleAvoidance _obs;
    private SteeringType _obsEnum;
    private Transform _self;
    private float _desiredSpeed;
    private Func<bool> _canShoot;
    private Action<Vector3> _onRotate;
    public EnemyChaseState(Action<Vector3, float> onChase,Action<Vector3> onRotate, Func<bool> canShoot, INode root, ObstacleAvoidance obs, SteeringType obsEnum, Func<bool> isOnSight,Transform self ,float desiredSpeed)
    {
        _onChase = onChase;
        _root = root;
        _obs = obs;
        _obsEnum = obsEnum;
        _isOnSight = isOnSight;
        _self = self;
        _desiredSpeed = desiredSpeed;
        _canShoot = canShoot;
        _onRotate = onRotate;
    }

    public override void Awake()
    {
        _obs.SetActualBehaviour(_obsEnum);

    }
    public override void Execute()
    {
        if (!_isOnSight())
        {
            _root.Execute();
            return;
        }
        if (_isOnSight() && _canShoot())
        {
            _root.Execute();
            return;
        }
        _onChase?.Invoke(_self.forward,_desiredSpeed);
        _onRotate?.Invoke(_obs.GetFixedDir());
    }

}
