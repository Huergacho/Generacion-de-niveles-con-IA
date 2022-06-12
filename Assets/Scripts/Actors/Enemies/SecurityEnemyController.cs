using System;
using UnityEngine;

[RequireComponent(typeof(SecurityEnemyModel))]
public class SecurityEnemyController : BaseEnemyController
{
    [SerializeField] private Transform[] wayPoints;
    private SecurityEnemyModel _model;
    //private SecurityEnemyView _enemyView;

    //Events
    public event Action<bool> onDetect;

    protected void Awake()
    {
        _model = GetComponent<SecurityEnemyModel>();
        //_enemyView = GetComponent<SecurityEnemyView>();
    }

    protected override void Start()
    {
        //_enemyView.SuscribeEvents(this);
        base.Start();
    }

    protected virtual void InitBehaviours()
    {
        var seek =  new Seek(_model.Target, transform);
        _model.Behaviours.Add(SteeringType.Seek,seek);
        var pursuit = new Chase(transform, _model.Target, _model.IAStats.PredictionTime);
        _model.Behaviours.Add(SteeringType.Chase, pursuit);
    }

    protected override void InitFSM() //TODO: FIX FSM
    {
        //var idle = new EnemyIdleStates<enemyStates>(_model, IdleCommand, _root );
        var patrol = new EnemyPatrolState<enemyStates>(_model, _root, wayPoints, SteeringType.Seek, DetectCommand);
        var chase = new EnemyChaseState<enemyStates>(_model, CheckForShooting, _root, SteeringType.Chase, DetectCommand);
        var shoot = new EnemyShootState<enemyStates>(_model, _root, SteeringType.Seek, DetectCommand, CheckForShooting);

        //idle.AddTransition(enemyStates.Patrol, patrol);
        //idle.AddTransition(enemyStates.Chase, chase);

        //chase.AddTransition(enemyStates.Idle, idle);
        chase.AddTransition(enemyStates.Shoot, shoot);
        chase.AddTransition(enemyStates.Patrol, patrol);

        //patrol.AddTransition(enemyStates.Idle, idle);
        patrol.AddTransition(enemyStates.Chase, chase);
        patrol.AddTransition(enemyStates.Shoot, shoot);

        shoot.AddTransition(enemyStates.Chase, chase);
        shoot.AddTransition(enemyStates.Patrol, patrol);

        _fsm = new FSM<enemyStates>(patrol);
    }
    protected override void InitDesitionTree() 
    { 

        INode chase = new ActionNode(() => _fsm.Transition(enemyStates.Chase));
        INode patrol = new ActionNode(() => _fsm.Transition(enemyStates.Patrol));
        //INode idle = new ActionNode(() => _fsm.Transition(enemyStates.Idle));
        INode shoot = new ActionNode(() => _fsm.Transition(enemyStates.Shoot));
        INode pursuit = new ActionNode(() => _fsm.Transition(enemyStates.Chase));

        //LOGIC: Is Player dead? -> Have I Taken Damage-> Can I See You? -> Can I Attack You?
        INode QCanShoot = new QuestionNode(CheckForShooting, shoot, chase);
        INode QOnSight = new QuestionNode(DetectCommand, QCanShoot, patrol);
        INode QReceivedDamage = new QuestionNode(() => _model.HasTakenDamage, pursuit, QOnSight); //if i have damage, then pursuit player.
        INode QPlayerAlive = new QuestionNode(_model.CheckForPlayer, QReceivedDamage, patrol);
        _root = QPlayerAlive;
    }

    private bool DetectCommand()
    {
        if (!_model.LineOfSight.CheckForOneTarget())
        {
            onDetect?.Invoke(false);

            return false;
        }

        onDetect?.Invoke(true);
        return true;

    }

    private void IdleCommand()
    {
        _model.Move(transform.forward, 0);
    }

    private bool CheckForShooting()
    {
        var distance = Vector3.Distance(transform.position, _model.Target.transform.position);
        if(distance > _model.IAStats.ShootDistance)
        {
            return false;
        }

        return true;
    }

    public void OnDrawGizmosSelected()
    {
        if (_model == null) 
        {
            Debug.LogWarning("Model is null");
            return;
        }

        Gizmos.color = Color.red;
        if(_model?.Avoidance != null && _model?.Avoidance.ActualBehaviour != null)
        {
            var dir = _model.Avoidance.ActualBehaviour.GetDir(); 
            Gizmos.DrawRay(transform.position, dir * 2);
        }
        Gizmos.DrawWireSphere(transform.position, _model.IAStats.RangeAvoidance);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, _model.ActorStats.AngleVision / 2, 0) * transform.forward * _model.ActorStats.RangeVision);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -_model.ActorStats.AngleVision / 2, 0) * transform.forward * _model.ActorStats.RangeVision);
    }
}
