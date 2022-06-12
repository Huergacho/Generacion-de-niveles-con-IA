using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    private int collectableItemCounter;

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
            DontDestroyOnLoad(gameObject);
        }
    }

    public void CollectablePickUp(int value)
    {
        collectableItemCounter += value;
        OnCollectable?.Invoke(collectableItemCounter);
    }
}
