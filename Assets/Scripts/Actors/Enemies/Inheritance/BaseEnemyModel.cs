using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(LineOfSight))]
public abstract class BaseEnemyModel : EntityModel, IArtificialMovement
{
    [SerializeField] protected ObstacleAvoidanceSO obstacleAvoidanceSO;
    [SerializeField] protected IAStats _iaStats;
    [SerializeField] protected Transform _firePoint;
    [SerializeField] protected LazerGun _gun;
    
    protected LineOfSight _lineOfSight;
    protected Dictionary<SteeringType, ISteering> behaviours = new Dictionary<SteeringType, ISteering>();
    protected PlayerModel _actualTarget = null;
    protected ObstacleAvoidance _obstacleAvoidance;

    public Dictionary<SteeringType, ISteering> Behaviours => behaviours;
    public ObstacleAvoidance Avoidance => _obstacleAvoidance;
    public PlayerModel Target { get; private set; }
    public IAStats IAStats => _iaStats;
    public LineOfSight LineOfSight => _lineOfSight;

    protected override void Awake()
    {
        base.Awake();
        _lineOfSight = GetComponent<LineOfSight>();
        InitBehaviours();
    }

    protected virtual void Start()
    {
        GameManager.instance.OnPlayerInit += OnPlayerInit;
    }

    protected virtual void OnPlayerInit(PlayerModel player)
    {
        GameManager.instance.OnPlayerInit -= OnPlayerInit;
        _actualTarget = player;
        _obstacleAvoidance = new ObstacleAvoidance(this);
    }

    protected abstract void InitBehaviours();

    protected virtual void Shoot()
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
        return !_actualTarget.LifeController.IsDead;
    }
}
