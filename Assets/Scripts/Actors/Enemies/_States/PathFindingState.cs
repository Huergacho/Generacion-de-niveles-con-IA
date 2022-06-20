using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingState<T> : State<T>
{
    IArtificialMovement _self;
    private INode _root;
    private SteeringType _obsEnum;

    private Astar<Vector3> _ast;
    private List<Vector3> _path;
    private Vector3 _homePos;
    private float _homeRange;
    private int _index;
    private float _minDistance = 0.5f;

    public PathFindingState(IArtificialMovement self, INode root, SteeringType obsEnum, float homeRange)
    {
        _self = self;
        _obsEnum = obsEnum;
        _root = root;

        _homeRange = homeRange;
        _ast = new Astar<Vector3>();
    }

    public override void Awake()
    {
        _self.LifeController.OnTakeDamage += TakeHit;
        _homePos = _self.PatrolRoute[0].transform.position;
        _self.Avoidance.SetActualBehaviour(_obsEnum);
        //_self.Avoidance.ActualBehaviour.SetTarget(_self.Target); //Lets set the player as target;

        SetPath();
    }

    public override void Execute()
    {
        if (_self.IsTargetInSight() || !_self.FarFromHome() || _path == null || _path.Count < 2) //If any of this are true then... 
        {
            _root.Execute();
            return;
        }

        RunList();
    }

    #region PathFinding
    void SetPath()
    {
        var startPos = _self.transform.position;
        _path = _ast.GetPath(startPos, IsSatisfied, GetNeighbours, GetCost, Heuristic);
        _index = 0;
    }

    void RunList()
    {
        Vector3 currPoint = _path[_index];
        currPoint.y = _self.transform.position.y;
        Vector3 diff = currPoint - _self.transform.position;
        Vector3 dir = diff.normalized;

        float distance = diff.magnitude;
        if (distance < _minDistance && _index < (_path.Count - 1))
            _index++;

        if (_index >= (_path.Count - 1))
        {
            _root.Execute();
            return;
        }

        _self.LookDir(_self.Avoidance.GetDir(dir));
        _self.Move(_self.transform.forward, _self.ActorStats.WalkSpeed);
    }

    float Heuristic(Vector3 curr)
    {
        float heuristic = 0;
        heuristic += Vector3.Distance(curr, _homePos);
        return heuristic;
    }

    float GetCost(Vector3 from, Vector3 to)
    {
        float distanceMultiplier = 1;
        float cost = 0;
        cost += Vector3.Distance(from, to) * distanceMultiplier;
        return cost;
    }

    List<Vector3> GetNeighbours(Vector3 curr)
    {
        List<Vector3> neighbours = new List<Vector3>();
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if (x == 0 && z == 0) continue;
                var newPos = new Vector3(curr.x + x, curr.y, curr.z + z);
                Vector3 diff = newPos - curr;
                Vector3 dir = diff.normalized;
                float distance = diff.magnitude;
                if (Physics.Raycast(curr, dir, distance, _self.ActorStats.ObstacleLayers)) continue;
                neighbours.Add(newPos);
            }
        }
        return neighbours;
    }

    bool IsSatisfied(Vector3 curr)
    {
        float distance = Vector3.Distance(curr, _homePos);
        return distance <= _homeRange;
    }
    #endregion

    private void TakeHit()
    {
        _self.TakeHit(true);
        _root.Execute();
    }

    public override void Sleep()
    {
        _self.LifeController.OnTakeDamage -= TakeHit;
        _path = null;
    }
}
