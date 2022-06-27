using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractableController), typeof(RoomActor))]
public class CollectableItem : MonoBehaviour, IItem
{
    [SerializeField] private int value = 1;
    [SerializeField] private bool isStolen;

    private RoomActor roomActor;
    private MeshRenderer _meshRenderer;
    private Collider _collider;

    public RoomActor RoomActor => roomActor;

    private void Start()
    {
        roomActor = GetComponent<RoomActor>();
        _collider = GetComponent<Collider>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Interact(PlayerModel player)
    {
        LevelManager.instance.CollectablePickUp(value);
        Destroy(gameObject);
    }

    public void StealItem()
    {
        isStolen = true;
        ShowItem(!isStolen);
    }

    public void DropItem(Vector3 position)     
    {
        isStolen = false;
        ShowItem(!isStolen);
        gameObject.transform.position = position;
    }

    public void ShowItem(bool value)
    {
        _meshRenderer.enabled = value;
        _collider.enabled = value;
    }
}
