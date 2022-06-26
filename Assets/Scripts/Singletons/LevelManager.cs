using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField] private Transform enemyNodesParent;
    [SerializeField] private double timer = 180f;
    private Room _currentRoom;
    private int collectableItemCounter;

    //Propierties
    public Transform PatrolNodeParent => enemyNodesParent;
    public List<IStealable> Items => CurrentRoom.Items;

    public LevelGenerator LevelGenerator { get; private set; }

    public Room CurrentRoom => _currentRoom;

    //Events
    public Action<int> OnCollectable;
    public Action<double> OnTimerUpdate;

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

        timer += 1f; //cuz when it starts, there is always a little delay
    }

    private void Start()
    {
        StartCoroutine(RunTimer());
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

    private IEnumerator RunTimer()
    {
        Double count = timer;
        while(count > 0)
        {
            count--;
            OnTimerUpdate?.Invoke(count);
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(0.5f);
        GameManager.instance.IsSceneReadyToChange = true;
        GameManager.instance.GameOver();
    }
}
