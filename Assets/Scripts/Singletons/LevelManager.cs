using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField] private Transform enemyNodesParent;
    private Room _currentRoom;
    private int collectableItemCounter;

    //Propierties
    public Transform PatrolNodeParent => enemyNodesParent;
    public List<IStealable> Items => CurrentRoom.Items;

    public LevelGenerator LevelGenerator { get; private set; }

    public Room CurrentRoom => _currentRoom;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CurrentRoom.OpenRoom();
        }
    }

    public void CollectablePickUp(int value)
    {
        collectableItemCounter += value;
        OnCollectable?.Invoke(collectableItemCounter);
    }

    public void SetLevelGenerator(LevelGenerator levelGenerator)
    {
        LevelGenerator = levelGenerator;
    }

    public void SetCurrentLastOpenedRoom(Room currentRoom)
    {
        _currentRoom = currentRoom;
        CurrentRoom.IsOpen = true;
    }
}
