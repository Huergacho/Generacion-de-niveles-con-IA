using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private Astar<RoomNode> _aStar;
    private List<RoomNode> gridsPositions;
    [SerializeField] private float limitDistance;
    private RoomNode startPos;
    private RoomNode endPos;
    private int watchDog;
    public void Start()
    {
        _aStar = new Astar<RoomNode>();
    }
    public void AStarVectorCommand()
    {
        startPos = gridsPositions[Random.Range(0, gridsPositions.Count)];
        endPos = gridsPositions[Random.Range(0, gridsPositions.Count)];
        while (endPos == startPos || watchDog > 100)
        {
            endPos = gridsPositions[Random.Range(0, gridsPositions.Count)];
            watchDog++;

        }
        List<RoomNode> path = _aStar.GetPath(startPos, CheckNode,GetNeighbours,GetCost,GetHeuristic);
        GenerateLevel(path);

    }
    public void GetPath()
    {

    }
    bool CheckNode(RoomNode check)
    {
        var dist = Vector3.Distance(check.transform.position, endPos.transform.position);
        return dist < limitDistance;
    }
    List<RoomNode> GetNeighbours(RoomNode curr)
    {
         List<RoomNode> roomList = new List<RoomNode>();
        foreach (var item in curr.GetNeightbourd())
        {
            roomList.Add(item);
        }
        return roomList;
    }
    float GetCost(RoomNode parent, RoomNode child)
    {

        float cost = 0;
        cost += Vector3.Distance(parent.transform.position, child.transform.position);
        return cost;
    }
    float GetHeuristic(RoomNode curr)
    {
        float distanceMultiplier = 2;

        float h = 0;
        h += Vector3.Distance(curr.transform.position, endPos.transform.position) + distanceMultiplier;

        return h;
    }
    void GenerateLevel(List<RoomNode> roomsToActivate)
    {
        foreach (var item in roomsToActivate)
        {
            if (gridsPositions.Contains(item))
            {
                item.gameObject.SetActive(true);
            }
            else
            {
                continue;
            }
        }
    }
}
