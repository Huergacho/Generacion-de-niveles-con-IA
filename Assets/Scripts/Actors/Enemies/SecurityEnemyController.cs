using System;
using UnityEngine;
public class SecurityEnemyController : BaseEnemyController
{
    enum enemyStates
    {
        Idle,
        Patrol,
        Seek,
        Chase,
        Shoot
    }

    [SerializeField] private Transform[] wayPoints;
    private SecurityEnemyModel _model;
    //private SecurityEnemyView _enemyView;
    private FSM<enemyStates> _fsm;

    public event Action<Vector3> onRotate;
    public event Action<Vector3, float> onMove;
    public event Action<bool> onDetect;
    public event Action onDie;
    public event Action onShoot;

    protected void Awake()
    {
        _model = GetComponent<SecurityEnemyModel>();
        //_enemyView = GetComponent<SecurityEnemyView>();
    }

    protected override void Start()
    {
        //_enemyModel.SuscribeEvents(this);
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
        var patrol = new EnemyPatrolState<enemyStates>(_model, _root, wayPoints, SteeringType.Seek, DetectCommand);
        var chase = new EnemyChaseState<enemyStates>(MovementCommand, RotateCommand, CheckForShooting, _root, _model.Avoidance, SteeringType.Chase, DetectCommand, transform, _model.ActorStats.RunSpeed);
        var shoot = new EnemyShootState<enemyStates>(Shoot, MovementCommand, RotateCommand, _root, _model.Avoidance, SteeringType.Seek, DetectCommand, CheckForShooting, transform, _model.ActorStats.WalkSpeed);

        //idle.AddTransition(enemyStates.Patrol, patrol);
        //idle.AddTransition(enemyStates.Chase, chase);


        ////chase.AddTransition(enemyStates.Idle, idle);
        //chase.AddTransition(enemyStates.Shoot, shoot);
        //chase.AddTransition(enemyStates.Patrol, patrol);

        ////patrol.AddTransition(enemyStates.Idle, idle);
        //patrol.AddTransition(enemyStates.Chase, chase);
        //patrol.AddTransition(enemyStates.Shoot, shoot);

        //shoot.AddTransition(enemyStates.Chase, chase);
        //shoot.AddTransition(enemyStates.Patrol, patrol);



        //_fsm = new FSM<enemyStates>(patrol);
    }
    protected override void InitDesitionTree() 
    { 

        INode chase = new ActionNode(() => _fsm.Transition(enemyStates.Chase));
        INode patrol = new ActionNode(() => _fsm.Transition(enemyStates.Patrol));
        //INode idle = new ActionNode(() => _fsm.Transition(enemyStates.Idle));
        INode shoot = new ActionNode(() => _fsm.Transition(enemyStates.Shoot));

        INode QCanShoot = new QuestionNode(CheckForShooting, shoot, chase);
        INode QOnSight = new QuestionNode(DetectCommand, QCanShoot, patrol);
        INode QPlayerAlive = new QuestionNode(_model.CheckForPlayer, QOnSight, patrol);
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
        onMove(transform.forward, 0);
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

    private void Shoot()
    {
        onShoot?.Invoke();
    }

    private void MovementCommand(Vector3 moveDir, float desiredSpeed)
    {
        onMove?.Invoke(moveDir, desiredSpeed);
    }

    private void RotateCommand(Vector3 rotationDir)
    {
        onRotate?.Invoke(_model.Avoidance.GetFixedDir(rotationDir));
    }

    public void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;
        if(_model.Avoidance != null && _model.Avoidance.ActualBehaviour != null)
        {
            var dir = _model.Avoidance.ActualBehaviour.GetDir(); 
            Gizmos.DrawRay(transform.position, dir * 2);
        }
        Gizmos.DrawWireSphere(transform.position, _model.IAStats.RangeAvoidance);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, _model.IAStats.AngleAvoidance / 2, 0) * transform.forward * _model.IAStats.RangeAvoidance);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -_model.IAStats.AngleAvoidance / 2, 0) * transform.forward * _model.IAStats.RangeAvoidance);
    }

    private void Update()
    {
        _fsm.UpdateState();
    }
}
