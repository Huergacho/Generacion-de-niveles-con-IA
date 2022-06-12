using System;
using UnityEngine;

public class EnemyPatrolState<T> : State<T>
{
    private IArtificialMovement _ia;
    private INode _root;
    private SteeringType _obsEnum;
    private Func<bool> _isOnSight;
    private Transform[] _wayPoints;
    private Transform currWayPoint;
    private int currentWayPointIndex = 0;

    public EnemyPatrolState(IArtificialMovement ia, INode root, Transform[] wayPoints, SteeringType obsEnum, Func<bool> isOnSight)
    {
        _ia = ia;
        _root = root;
        _wayPoints = wayPoints;
        _obsEnum = obsEnum;
        _isOnSight = isOnSight;
    }

    public override void Awake()
    {
        _ia.LifeController.OnTakeDamage += TakeHit;
        _ia.Avoidance.SetActualBehaviour(_obsEnum); //TODO: its somehow broken????
    }

    public override void Execute() //TODO: Add to check that if receives damage, the start chasing?
    {
        if (_isOnSight())
        {
            _root.Execute();
            return;
        }

        CheckWayPoint();

        _ia.Move((currWayPoint.position - _ia.transform.position).normalized, _ia.ActorStats.RunSpeed);
        _ia.SmoothRotation((currWayPoint.position - _ia.transform.position).normalized);        
    }

    private void CheckWayPoint()
    {

        if (currWayPoint != _wayPoints[currentWayPointIndex])
        {
            currWayPoint = _wayPoints[currentWayPointIndex];
        }

        var posWay = currWayPoint.position;
        posWay.y = _ia.transform.position.y;
        var distance = Vector3.Distance(_ia.transform.position, posWay);

        if (distance <= 0.25f)
        {
            if (currWayPoint == _wayPoints[_wayPoints.Length - 1])
            {
                currentWayPointIndex = 0;
                currWayPoint = _wayPoints[currentWayPointIndex];
            }
            else
            {
                currentWayPointIndex++;
                currWayPoint = _wayPoints[currentWayPointIndex];
            }
        }
    }

    private void TakeHit()
    {
        _ia.TakeHit();
        _root.Execute();
    }

    public override void Sleep()
    {
        _ia.LifeController.OnTakeDamage -= TakeHit;
    }
}
