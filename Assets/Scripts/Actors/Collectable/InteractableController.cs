using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableController : MonoBehaviour
{
    public IItem interactable;

    [SerializeField] private bool isTrigger = true;

    private CollectableItem stealable;
    private Collider _collider;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        if (_collider != null)
            _collider.isTrigger = isTrigger;
        else
            Debug.LogWarning($"{gameObject.name} Collider is missing!");

        stealable = GetComponent<CollectableItem>();
        interactable = GetComponent<IItem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        DoYourThing(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var collider = GetComponent<Collider>();
        if (collider != null)
            DoYourThing(collider);
    }

    private void DoYourThing(Collider other)
    {
        PlayerModel player = other.GetComponent<PlayerModel>();
        IThief thief = other.GetComponent<IThief>();

        if (player != null)
        {
            interactable.Interact(player);
        }
        else if (stealable != null && thief != null)
        {
            thief.StealItem(stealable);
        }
    }
}
