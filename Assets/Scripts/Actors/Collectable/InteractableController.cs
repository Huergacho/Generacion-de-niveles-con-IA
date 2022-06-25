using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableController : MonoBehaviour
{
    public IItem interactable;
    [SerializeField] private bool isTrigger = true;

    private void Start()
    {
        var collider = GetComponent<Collider>();
        if (collider != null)
            collider.isTrigger = isTrigger;
        else
            Debug.LogWarning($"{gameObject.name} Collider is missing!");
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerModel>();

        if (player) //Si es un player
        {
            interactable.Interact(player);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            GameManager.instance.Victory();
        }
    }
}
