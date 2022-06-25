using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefEnemyModel : BaseEnemyModel, IThief
{
    private IStealable _itemTarget;
    private IStealable _itemStolen;
    public IStealable ItemStolen => _itemStolen;
    public IStealable ItemTarget => _itemTarget; 

    public void StealItem(IStealable item)
    {
        if(_itemStolen == null)
            _itemStolen = item;

        //TODO: maybe do a puff or animation or sound when de item is stolen?
        ShowItem(false);
    }

    public void DropStolenItem()
    {
        if (_itemStolen == null) return;

        //TODO: maybe do a puff or animation or sound when de item is dropped? 
        _itemStolen.transform.position = transform.position;
        ShowItem(true);
        _itemStolen = null;
    }

    private void ShowItem(bool value)
    {
        _itemStolen.gameObject.SetActive(value);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        DropStolenItem();
    }
}
