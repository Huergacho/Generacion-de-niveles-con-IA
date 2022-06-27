using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RoomActor))]
public class StealableScript : MonoBehaviour, IStealable
{
    [SerializeField] private bool isStolen;

    private MeshRenderer _meshRenderer;
    private Collider _collider;

    void Start()
    {
        _collider = GetComponent<Collider>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void StealItem() //TODO: maybe do a puff or animation or sound when de item is stolen?
    {
        isStolen = true;
        ShowItem(!isStolen);
    }

    public void DropItem(Vector3 position) //TODO: maybe do a puff or animation or sound when de item is dropped?       
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
