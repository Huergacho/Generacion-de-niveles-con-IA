using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractableController))]
public class CollectableItem : MonoBehaviour, IItem
{
    [SerializeField] private int value = 1;

    public void Interact(PlayerModel player)
    {
        LevelManager.instance.CollectablePickUp(value);
        Destroy(gameObject);
    }
}
