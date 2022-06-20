using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealableScript : MonoBehaviour, IStealable
{
    void Start()
    {
        LevelManager.instance.AddItem(this);
    }
}
