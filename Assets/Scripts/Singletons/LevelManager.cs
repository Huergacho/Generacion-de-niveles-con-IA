using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField] private Transform enemyNodesParent;
    [SerializeField] private LevelGenerator _levelGenerator;
    private int collectableItemCounter;

    //Propierties
    public Transform PatrolNodeParent => enemyNodesParent;
    public List<IStealable> Items => CurrentRoom.Items;

    public LevelGenerator LevelGenerator { get; private set; }

    public Room CurrentRoom { get; private set; }

    //Events
    public Action<int> OnCollectable;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void CollectablePickUp(int value)
    {
        collectableItemCounter += value;
        OnCollectable?.Invoke(collectableItemCounter);
    }

    public void SetLevelGenerator(LevelGenerator levelGenerator)
    {
        _levelGenerator = levelGenerator;
    }

    public void SetCurrentLastOpenedRoom(Room currentRoom)
    {
        CurrentRoom = currentRoom;
    }
}
