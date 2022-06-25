using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField] private Transform enemyNodesParent;
    private int collectableItemCounter;
    private List<IStealable> _itemsInLevel = new List<IStealable>(); //if later on we need to check items per room, then we do a dictionary?

    //Propierties
    public Transform PatrolNodeParent => enemyNodesParent;
    public List<IStealable> Items => _itemsInLevel;

    public Transform PlayerSpawnPoint { get; set; }
    public GameObject victoryItem { get; set; }

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

    public void AddItem(IStealable item)
    {
        if(!_itemsInLevel.Contains(item)) //si el item no estaba ya en el listado... (cuenta los repetidos???? no deberia)
            _itemsInLevel.Add(item);
    }
}
