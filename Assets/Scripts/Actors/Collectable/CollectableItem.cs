using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractableController), typeof(RoomActor))]
public class CollectableItem : MonoBehaviour, IItem
{
    [SerializeField] private int value = 1;
    private RoomActor roomActor;

    public RoomActor RoomActor => roomActor;

    private void Start()
    {
        roomActor = GetComponent<RoomActor>();
    }

    public void Interact(PlayerModel player)
    {
        LevelManager.instance.CollectablePickUp(value);
        Destroy(gameObject);
    }
}
