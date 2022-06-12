using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LifeController))]
public class EntityModel : MonoBehaviour
{
    [SerializeField] protected ActorStats _actorStats;

    //Propierties
    public ActorStats ActorStats => _actorStats;
    public LifeController LifeController { get; private set; }

    public Action OnDie;

    protected virtual void Awake()
    {
        LifeController = GetComponent<LifeController>();
        LifeController.SetMaxLife(_actorStats.MaxLife);
        LifeController.OnDie += Die;
    }

    public virtual void Die()
    {
        OnDie?.Invoke();
        Destroy(gameObject); //When they die.. destroy.
    }
}
