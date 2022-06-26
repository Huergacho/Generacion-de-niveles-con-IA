using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(LineOfSight), typeof(RoomActor))]
public abstract class BaseEnemyModel : EntityModel, IArtificialMovement
{
    [SerializeField] protected IAStats _iaStats;
    [SerializeField] protected Transform _firePoint;
    [SerializeField] protected bool drawGizmos;
    protected bool hasTakenDamage;

    protected LazerGun _gun;
    protected Dictionary<SteeringType, ISteering> behaviours = new Dictionary<SteeringType, ISteering>();
    protected ObstacleAvoidance _obstacleAvoidance;
    protected RoomActor roomActor;

    public Dictionary<SteeringType, ISteering> Behaviours => behaviours;
    public ObstacleAvoidance Avoidance { get; private set; }
    public PlayerModel Target { get; private set; }
    public IAStats IAStats => _iaStats;
    public LineOfSight LineOfSight { get; private set; }
    public bool HasTakenDamage => hasTakenDamage;
    public GameObject[] PatrolRoute { get; private set; }
    public Vector3 Destination { get; protected set; }
    public RoomActor RoomActor => roomActor;

    //Events
    public Action<bool> OnDetect { get => _onDetect; set => _onDetect = value; } //Este modo me lo mostro el profe para poder hacer que tuvieran eventos las interfaces.. dejalo asi?
    private Action<bool> _onDetect = delegate { };

    protected override void Awake()
    {
        base.Awake();
        LineOfSight = GetComponent<LineOfSight>();
        Avoidance = new ObstacleAvoidance(this);
        roomActor = GetComponent<RoomActor>();
        Destination = transform.position;
        GameManager.instance.OnPlayerInit += OnPlayerInit;
    }

    protected virtual void Start()
    {
        LifeController.OnTakeDamage += TakeDamage;
        var patrol = GetComponentInChildren<PatrolRoute>();
        if (patrol != null)
        {
            patrol?.Initialize();
            PatrolRoute = patrol.PatrolNodes;
        }
    }

    protected virtual void OnPlayerInit(PlayerModel player)
    {
        GameManager.instance.OnPlayerInit -= OnPlayerInit;
        Target = player;
    }
    public virtual void Shoot() //TODO: Habria que determinar si esto se queda aca o no porque el player tambien lo tiene pero si no todos los enemgios van disparar...
    {
        _gun.Shoot(_firePoint.position, _firePoint.forward);
    }

    public virtual void LookDir(Vector3 dir)
    {
        dir.y = 0;
        transform.forward = Vector3.Lerp(transform.forward, dir, ActorStats.RotSpeed);
    }

    public bool IsPlayerDead()
    {
        return GameManager.instance.Player.LifeController.IsDead;
    }

    public void TakeHit(bool value)
    {
        hasTakenDamage = value; 
    }

    private void TakeDamage()
    {
        TakeHit(true);
    }

    private void DropCollectable()
    {
        if(IAStats.CollectablePrefab != null)
        {
            var collectable = Instantiate(IAStats.CollectablePrefab);
            collectable.GetComponent<RoomActor>().SetRoomReference(RoomActor.RoomReference);
            collectable.transform.position = transform.position;
        }
    }

    public bool IsTargetInSight()
    {
        bool value = LineOfSight.CheckForOneTarget();
        OnDetect?.Invoke(value);
        return value;
    }

    public bool IsInShootingRange()
    {
        var distance = Vector3.Distance(transform.position, Target.transform.position);
        //print($"Is in shooting range?: distance: {distance} Stats: {IAStats.ShootDistance} result: {distance <= IAStats.ShootDistance}");
        return distance <= IAStats.ShootDistance;
    }

    public bool FarFromDestination()
    {
        var distance = Vector3.Distance(transform.position, Destination); 
        return distance >= IAStats.NearTargetRange;
    }

    public bool IsEnemyFar()
    {
        var distance = Vector3.Distance(transform.position, Target.transform.position);
        return distance >= IAStats.NearTargetRange;
    }

    public override void Die()
    {
        base.Die();
        DropCollectable();
        RoomActor.OnDie();
        var patrol = GetComponentInChildren<PatrolRoute>();
        if (patrol != null)
        {
            patrol.DestroyPatrolNodes();
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        LifeController.OnTakeDamage -= TakeDamage;
    }

    public void OnDrawGizmosSelected()
    {
        if (!drawGizmos) return;

        Gizmos.color = Color.red;
        if (Avoidance != null && Avoidance.ActualBehaviour != null)
        {
            var dir = Avoidance.ActualBehaviour.GetDir();
            Gizmos.DrawRay(transform.position, dir * 2);
        }

        Gizmos.DrawWireSphere(transform.position, IAStats.RangeAvoidance);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, ActorStats.RangeVision);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, ActorStats.AngleVision / 2, 0) * transform.forward * ActorStats.RangeVision);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -ActorStats.AngleVision / 2, 0) * transform.forward * ActorStats.RangeVision);
    }
}
