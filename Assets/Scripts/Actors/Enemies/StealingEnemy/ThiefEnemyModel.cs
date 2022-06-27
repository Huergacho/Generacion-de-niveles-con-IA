using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefEnemyModel : BaseEnemyModel, IThief
{
    [SerializeField] private GameObject topOfHeadCoin;

    private Transform escapePoint;

    private CollectableItem _itemTarget;
    private CollectableItem _itemStolen;
    public CollectableItem ItemStolen => _itemStolen;
    public CollectableItem NextTarget => _itemTarget;

    public void Initialize()
    {
        topOfHeadCoin.SetActive(false);
        GameManager.instance.OnPlayerInit -= OnPlayerInit;
        Target = GameManager.instance.Player;
    }

    public void StealItem(CollectableItem item)
    {
        if (_itemStolen != null) return;
        _itemStolen = item;

        _itemStolen.StealItem();
        topOfHeadCoin.SetActive(true);
    }

    public void DropStolenItem()
    {
        if (_itemStolen == null) return;
        topOfHeadCoin.SetActive(false);
        _itemStolen.DropItem(transform.position);
        _itemStolen = null;
    }

    public bool IsThereAnItemToSteal() //if there is a item in the room to steal
    {
        GetATarget();
        return NextTarget != null;
    }

    public void GetATarget()
    {
        print("Let´s get a Target");
        if (_itemStolen == null && NextTarget == null && LevelManager.instance.Items.Count > 0)
        {
            foreach (var item in LevelManager.instance.Items)
            {
                if(item.RoomActor.RoomReference == RoomActor.RoomReference)
                    SetNextTarget(item);
            }
            print("SELECTED SOMETHING? " + NextTarget.transform.position);
        }
    }

    public void SetNextTarget(CollectableItem item)
    {
        _itemTarget = item;
        Destination = item.transform.position;
    }

    public void SetEscapePoint(Transform location)
    {
        escapePoint = location;
        transform.position = escapePoint.position;
        ReturnHomeDestination();
    }

    public void ReturnHomeDestination()
    {
        Destination = escapePoint.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        CollectableItem item = collision.gameObject.GetComponent<CollectableItem>();
        if (item != null)
        {
            print("I stole a " + collision.gameObject.name);
            StealItem(item);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        DropStolenItem();
    }
}
