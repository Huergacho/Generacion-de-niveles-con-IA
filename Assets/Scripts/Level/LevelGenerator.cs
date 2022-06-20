using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Room[] levels;
    [SerializeField] private List<Room> AstarLevel;
    [SerializeField] private float limitDistance;
    private Astar<Room> _astar;
    [SerializeField]private Room _finalPoint;

    private void Start()
    {
        _astar = new Astar<Room>();
        RunLevelRoullete();
    }
    #region Bake
    public void BakeLevels()
    {
        foreach (Room item in levels)
        {
            MyEngine.MyRandom.Shuffle(levels);
        }
    }
    #endregion
    public void RunLevelRoullete()
    {

        var startPoint = levels[0];
        _finalPoint = levels[levels.Length - 1];
        AstarLevel =_astar.GetPath(startPoint, CheckRoom, GetNeightbours, GetCost, GetHeuristic);
        ShowPath();
    }
    private void ShowPath()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            var curr = levels[i];
            if (AstarLevel.Contains(curr))
            {
                continue;
            }
            else
            {
                curr.gameObject.SetActive(false);
            }
        }
    }
    #region PathFinding
    private bool CheckRoom(Room check)
    {
        var distance = Vector3.Distance(check.transform.position, _finalPoint.transform.position);
        return distance < limitDistance;
    }
    private List<Room> GetNeightbours(Room curr)
    {
        return curr.NeightBours;
    }
    float GetCost(Room parent, Room child)
    {
        float cost = 0;
        cost += Vector3.Distance(parent.transform.position, child.transform.position);
        return cost;
    }
    float GetHeuristic(Room curr)
    {
        float distanceMultiplier = 2;

        float h = 0;
        h += Vector3.Distance(curr.transform.position, _finalPoint.transform.position) + distanceMultiplier;

        return h;
    }

    #endregion


}
