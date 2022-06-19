using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LifeController), typeof(Rigidbody))]
public abstract class EntityModel : MonoBehaviour
{
    [SerializeField] protected ActorStats _actorStats;
    protected Rigidbody _rb;

    //Propierties
    public ActorStats ActorStats => _actorStats;
    public LifeController LifeController { get; private set; }

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        LifeController = GetComponent<LifeController>();
        LifeController.SetMaxLife(_actorStats.MaxLife);
        LifeController.OnDie += Die;
    }

    public void Move(Vector3 dir, float desiredSpeed)
    {
        _rb.velocity = new Vector3(desiredSpeed * dir.normalized.x, _rb.velocity.y, desiredSpeed * dir.normalized.z);
    }

    public void SmoothRotation(Vector3 dest)
    {
        var direction = (dest - transform.position);
        if (direction != Vector3.zero)
        {
            var rotDestiny = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotDestiny, ActorStats.RotSpeed * Time.deltaTime);
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject); //When they die.. destroy.
    }

    protected virtual void OnDestroy()
    {
        LifeController.OnDie -= Die;
    }
}
