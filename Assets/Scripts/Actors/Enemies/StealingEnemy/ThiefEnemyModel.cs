using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefEnemyModel : BaseEnemyModel, IThief
{
    private IStealable _itemTarget;
    private IStealable _itemStolen;
    public IStealable ItemStolen => _itemStolen;
    public IStealable NextTarget => _itemTarget;

    public void StealItem(IStealable item) //TODO: put item on enemy head somehow
    {
        if (_itemStolen != null) return;
        _itemStolen = item;

        _itemStolen.StealItem();
    }

    public void DropStolenItem()
    {
        if (_itemStolen == null) return;
        _itemStolen.DropItem(transform.position);
        _itemStolen = null;
    }

    public bool IsThereAnItemToSteal() //if there is a item in the room to steal, check from level manager?
    {
        if (_itemStolen == null && NextTarget == null && LevelManager.instance.Items.Count > 0)
        {
            SetNextTarget(LevelManager.instance.Items[0]); //TODO rework this when facu finish with the rooms.
        }

        return NextTarget != null;
    }

    public void SetNextTarget(IStealable item)
    {
        _itemTarget = item;
        Destination = item.transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        StealableScript item = collision.gameObject.GetComponent<StealableScript>();
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
