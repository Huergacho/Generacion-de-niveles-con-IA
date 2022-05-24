using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    [SerializeField] private Node startNode;
    [SerializeField] private Node endNode;
    [SerializeField] private float limitDistance = 1.5f;
    public CI_Model _model;
    public Box box;
    BFS<Node> _bfs;
    DFS<Node> _dfs;
    Djkstra<Node> _djkstra;
    Astar<Node> _astar;
    Astar<Vector3> _astarVector;
    Theta<Node> _theta;
    [SerializeField] private LayerMask obsMask;
    private void Awake()
    {
        _astarVector = new Astar<Vector3>();
        _bfs = new BFS<Node>();
        _dfs = new DFS<Node>();
        _djkstra = new Djkstra<Node>();
        _astar = new Astar<Node>();
        _theta = new Theta<Node>();
    }
    #region NodeCommands
    public void BFSCommand()
    {
        List<Node> path = _bfs.GetPath(startNode, CheckNode, GetNeighbours);
        print(path.Count == 0);
        _model.SetWayPoints(path);
        box.SetWayPoints(path);
    }
    public void DFSCommand()
    {
        List<Node> path = _dfs.GetPath(startNode, CheckNode, GetNeighbours);
        _model.SetWayPoints(path);
        box.SetWayPoints(path);
    }
    public void DjkstraCommand()
    {
        List<Node> path = _djkstra.GetPath(startNode, CheckNode, GetNeighbours, GetCost);

        _model.SetWayPoints(path);
        box.SetWayPoints(path);
    }
    public void AStarCommand()
    {
        List<Node> path = _astar.GetPath(startNode, CheckNode, GetNeighbours, GetCost, GetHeuristic);

        _model.SetWayPoints(path);
        box.SetWayPoints(path);
    }
    public void ThetaCommand()
    {
        List<Node> path = _theta.GetPath(startNode, CheckNode, GetNeighbours, GetCost, GetHeuristic, InView);

        _model.SetWayPoints(path);
        box.SetWayPoints(path);
    }
    public void AStarMinatorCommand()
    {
        List<Node> path = _astar.GetPath(startNode, CheckNode, GetNeighbours, GetCost, GetHeuristic);
        path = PathFilter(path);
        _model.SetWayPoints(path);
        box.SetWayPoints(path);
    }
    #endregion

    #region Vector Commands

    public void AStarVectorCommand()
    {
        Vector3 startPos = _model.transform.position;
        List<Vector3> path = _astarVector.GetPath(startPos, CheckNode, GetNeighbours, GetCost, GetHeuristic);
        _model.SetWayPoints(path);
        //box.SetWayPoints(path);
    }
    public void AStarMinatorVectorCommand()
    {
        Vector3 startPos = _model.transform.position;

        List<Vector3> path = _astarVector.GetPath(startPos, CheckNode, GetNeighbours, GetCost, GetHeuristic);
        path = PathFilter(path);
        _model.SetWayPoints(path);
        //box.SetWayPoints(path);
    }
    #endregion

    #region Vector Cal
    List<Vector3> PathFilter(List<Vector3> path)
    {
        if (path.Count <= 2) return path;

        var newPath = new List<Vector3>();
        newPath.Add(path[0]);
        for (int i = 1; i < path.Count - 1; i++)
        {
            if (InView(newPath[newPath.Count - 1], path[i])) continue;
            newPath.Add(path[i - 1]);
        }
        newPath.Add(path[path.Count - 1]);
        return newPath;
    }

    List<Vector3> GetNeighbours(Vector3 curr)
    {
        List<Vector3> neightBours = new List<Vector3>();
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if (x == 0 && z == 0) continue;
                var neigh = new Vector3(curr.x + x, curr.y, curr.z + z);
                if (!InView(curr, neigh)) continue;
                neightBours.Add(neigh);
            }
        }
        return neightBours;

    }

    bool CheckNode(Vector3 check)
    {
        var finishPos = box.transform.position;
        var dist = Vector3.Distance(check, finishPos);
        return dist < limitDistance;
    }

    bool InView(Vector3 gp, Vector3 gs)
    {
        Vector3 dir = gs - gp;
        return !Physics.Raycast(gp, dir.normalized, dir.magnitude, obsMask);
    }

    float GetCost(Vector3 parent, Vector3 child)
    {
        return 1f;
    }

    float GetHeuristic(Vector3 curr)
    {
        float distanceMultiplier = 2;

        float h = 0;
        h += Vector3.Distance(curr, box.transform.position) + distanceMultiplier;

        return h;
    }
    #endregion

    #region Node Cal
    List<Node> PathFilter(List<Node> path)
    {
        if (path.Count <= 2) return path;

        var newPath = new List<Node>();
        newPath.Add(path[0]);
        for (int i = 1; i < path.Count -1; i++)
        {
            if (InView(newPath[newPath.Count - 1], path[i])) continue;
            newPath.Add(path[i - 1]);
        }
        newPath.Add(path[path.Count - 1]);
        return newPath;
    }
    List<Node> GetNeighbours(Node curr)
    {
        return curr.neightbourds;
    }

    bool CheckNode(Node check)
    {
        if (endNode != check)
        {
            return false;
        }
        return true;
    }

    bool InView(Node gp, Node gs)
    {
        var gpPos = gp.transform.position;
        var gsPos = gs.transform.position;
        Vector3 dir = gsPos - gpPos;
        return !Physics.Raycast(gpPos, dir.normalized, dir.magnitude, obsMask);
    }

    float GetCost(Node parent, Node child)
    {
        float trapCost = 1; //Hacer esto un Scriptable Object con todos los distintos costos por si tenemos más factores que alternen

        float cost = 0;
        cost += Vector3.Distance(parent.transform.position, child.transform.position);
        if (child.hasTrap)
        {
            cost += trapCost;
        }
        return cost;
    }

    float GetHeuristic(Node curr)
    {
        float distanceMultiplier = 2;

        float h = 0;
        h += Vector3.Distance(curr.transform.position, endNode.transform.position) + distanceMultiplier;

        return h;
    }

    #endregion

}
