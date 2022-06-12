using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableController : MonoBehaviour
{
    public IItem interactable;
    [SerializeField] private bool isTrigger = true;

    private void Start()
    {
        GetComponent<Collider>().isTrigger = isTrigger;
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerModel>();

        if (player) //Si es un player
        {
            interactable.Interact(player);
        }
    }
}
