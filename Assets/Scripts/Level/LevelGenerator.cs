using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Room[] levels;
    [SerializeField] private List<Room> AstarLevel;
    [SerializeField] private float limitDistance;
    [SerializeField] private Room _finalPoint;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject _victoryItem;

    private Astar<Room> _astar;

    private void Start()
    {
        LevelManager.instance.SetLevelGenerator(this);
        LevelManager.instance.SetCurrentLastOpenedRoom(levels[0]);
        print("Starting room " + levels[0]);
    }

    public void RunLevelRoullete()
    {
        var startPoint = levels[0];
        _finalPoint = levels[levels.Length - 1];
        SetPlayerSpawnPoint(startPoint);
        SetVictoryItem(_finalPoint);
        AstarLevel =_astar.GetPath(startPoint, CheckRoom, GetNeightbours, GetCost, GetHeuristic);
        ShowPath();
    }

    #region Bake
    public void RunLevel()
    {
        _astar = new Astar<Room>();

        RunLevelRoullete();
    }

    public void BakeLevels()
    {
        foreach (var item in levels)
        {
            item.gameObject.SetActive(true);
            item.ResetLevel();

        }
        MyEngine.MyRandom.Shuffle(levels);
    }
    #endregion

    #region PathFinding
    private void ShowPath()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            var curr = levels[i];
            if (!AstarLevel.Contains(curr))
            {
                curr.gameObject.SetActive(false);
            }
        }
    }

    private bool CheckRoom(Room check)
    {
        var distance = Vector3.Distance(check.transform.position, _finalPoint.transform.position);
        return distance < limitDistance;
    }

    private List<Room> GetNeightbours(Room curr)
    {
        return curr.Neightbours;
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

    public void SetVictoryItem(Room finalRoom)
    {
        finalRoom.IsEndRoom = true;
        finalRoom.SetVictoryItem(_victoryItem);
    }

    public void SetPlayerSpawnPoint(Room statingRoom)
    {
        statingRoom.SetPlayer(player);
    }
}
