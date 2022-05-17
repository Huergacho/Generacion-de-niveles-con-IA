using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    public CI_Model _model;
    public Box box;
    [SerializeField] private Node startNode;
    [SerializeField] private Node endNode;
    BFS<Node> _bfs;
    DFS<Node> _dfs;
    Djkstra<Node> _djkstra;
    Astar<Node> _astar;
    private void Awake()
    {
        _bfs = new BFS<Node>();
        _dfs = new DFS<Node>();
        _djkstra = new Djkstra<Node>();
        _astar = new Astar<Node>();
    }
    private void Start()
    {

    }
    private void Update()
    {
    }
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
        List<Node> path = _astar.GetPath(startNode, CheckNode, GetNeighbours, GetCost,GetHeuristic);

        _model.SetWayPoints(path);
        box.SetWayPoints(path);
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
}
