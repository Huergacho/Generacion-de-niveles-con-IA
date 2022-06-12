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

    protected virtual void Awake()
    {
        LifeController = GetComponent<LifeController>();
        LifeController.SetMaxLife(_actorStats.MaxLife);
        LifeController.OnDie += OnDie;
    }

    public virtual void OnDie()
    {
        Destroy(gameObject); //When they die.. destroy.
    }
}
