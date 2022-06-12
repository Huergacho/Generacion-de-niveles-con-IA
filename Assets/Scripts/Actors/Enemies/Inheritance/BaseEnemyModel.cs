using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(LineOfSight), typeof(LazerGun))]
public abstract class BaseEnemyModel : EntityModel, IArtificialMovement
{
    [SerializeField] protected IAStats _iaStats;
    [SerializeField] protected Transform _firePoint;
    
    protected LazerGun _gun;
    protected Dictionary<SteeringType, ISteering> behaviours = new Dictionary<SteeringType, ISteering>();
    protected ObstacleAvoidance _obstacleAvoidance;

    public Dictionary<SteeringType, ISteering> Behaviours => behaviours;
    public ObstacleAvoidance Avoidance { get; private set; }
    public PlayerModel Target { get; private set; }
    public IAStats IAStats => _iaStats;
    public LineOfSight LineOfSight { get; private set; }
    public bool HasTakenDamage { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        _gun = GetComponent<LazerGun>();
        LineOfSight = GetComponent<LineOfSight>();
        InitBehaviours();
    }

    protected virtual void Start()
    {
        GameManager.instance.OnPlayerInit += OnPlayerInit;
    }

    protected virtual void OnPlayerInit(PlayerModel player)
    {
        GameManager.instance.OnPlayerInit -= OnPlayerInit;
        Target = player;
        Avoidance = new ObstacleAvoidance(this);
    }

    protected abstract void InitBehaviours();

    public virtual void Shoot()
    {
        _gun.Shoot(_firePoint.position, _firePoint.forward);
    }

    //protected virtual void LookDir(Vector3 dir) //No se esta usando;
    //{
    //    dir.y = 0;
    //    transform.forward = Vector3.Lerp(transform.forward, dir, ActorStats.RotSpeed);
    //}

    public bool CheckForPlayer()
    {
        return !Target.LifeController.IsDead;
    }

    public void TakeHit()
    {
        HasTakenDamage = true; 
    }
}
