using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RoomActor))]
public class StealableScript : MonoBehaviour, IStealable
{
    void Start()
    {
        //LevelManager.instance.AddItem(this);
    }
}
