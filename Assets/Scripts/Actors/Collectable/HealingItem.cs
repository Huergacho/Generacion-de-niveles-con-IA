using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractableController))]
public class HealingItem : MonoBehaviour, IItem
{
    [SerializeField] private float healing;

    public void Interact(PlayerModel player)
    {
        player.LifeController.Heal(healing);
        Destroy(gameObject);
    }
}
