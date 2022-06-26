using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(LazerGun))]
public class RangeEnemyModel : BaseEnemyModel
{
    protected override void Awake()
    {
        base.Awake();
        _gun = GetComponent<LazerGun>();
        _gun.SetGunCD(_actorStats.ShootCooldown);
    }
}