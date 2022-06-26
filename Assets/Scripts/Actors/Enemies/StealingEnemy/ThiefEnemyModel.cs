using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefEnemyModel : BaseEnemyModel, IThief
{
    [SerializeField] private GameObject topOfHeadCoin;

    private Transform escapePoint;

    private IStealable _itemTarget;
    private IStealable _itemStolen;
    public IStealable ItemStolen => _itemStolen;
    public IStealable NextTarget => _itemTarget;

    protected override void Awake()
    {
        base.Awake();
        topOfHeadCoin.SetActive(false);
    }

    public void StealItem(IStealable item)
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
        if (_itemStolen == null && NextTarget == null && LevelManager.instance.Items.Count > 0)
        {
            SetNextTarget(LevelManager.instance.Items[0]);
        }

        return NextTarget != null;
    }

    public void SetNextTarget(IStealable item)
    {
        _itemTarget = item;
        Destination = item.transform.position;
    }

    public void SetEscapePoint(Transform location)
    {
        escapePoint = location;
    }

    public void ReturnHomeDestination()
    {
        Destination = escapePoint.position;
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
