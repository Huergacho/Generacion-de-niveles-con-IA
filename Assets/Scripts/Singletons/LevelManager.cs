using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField] private Transform enemyNodesParent;
    private int collectableItemCounter;

    //Propierties
    public Transform PatrolNodeParent => enemyNodesParent;

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
            //DontDestroyOnLoad(gameObject);
        }
    }

    public void CollectablePickUp(int value)
    {
        collectableItemCounter += value;
        OnCollectable?.Invoke(collectableItemCounter);
    }
}
