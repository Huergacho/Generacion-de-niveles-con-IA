using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class EnemyShootState<T> : State<T>
{
    private Action _onShoot;
    private Action<Vector3, float> _onMove;
    private Action<Vector3> _onRotate;
    private INode _root;
    private Steerings _obsEnum;
    private ObstacleAvoidance _obstacleAvoidance;
    private Func<bool> _onDetect;
    private Func<bool> _onCheckDistance;
    Transform _self;
    float _desiredSpeed;
    public EnemyShootState(Action onShoot, Action<Vector3, float> onMove, Action<Vector3> onRotate, INode root,ObstacleAvoidance obstacleAvoidance ,Steerings steering, Func<bool> onDetect,Func<bool> onCheckDistance, Transform self, float desiredSpeed)
    {
        _onShoot = onShoot;
        _root = root;
        _obsEnum = steering;
        _obstacleAvoidance = obstacleAvoidance;
        _onDetect = onDetect;
        _onCheckDistance = onCheckDistance;
        _onMove = onMove;
        _self = self;
        _onRotate = onRotate;
        _desiredSpeed = desiredSpeed;
    }
    public override void Awake()
    {
        _obstacleAvoidance.SetActualBehaviour(_obsEnum);
    }
    public override void Execute()
    {
        if (!_onDetect())
        {
            _root.Execute();
            return;
        }
        if (!_onCheckDistance())
        {
            _root.Execute();
            return;
        }
        _onMove?.Invoke(_self.forward, _desiredSpeed);
        _onRotate?.Invoke(_obstacleAvoidance.GetFixedDir());
        _onShoot?.Invoke();
    }

}

