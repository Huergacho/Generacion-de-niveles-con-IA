using System;
using UnityEngine;

[RequireComponent(typeof(RangeEnemyModel))]
public class RangeEnemyController : BaseEnemyController
{
    [SerializeField] private Transform[] wayPoints;
    //private RangeEnemyModel _model;
    //private SecurityEnemyView _enemyView;

    protected override void Awake()
    {
        base.Awake();
        //_model = GetComponent<RangeEnemyModel>();
        //_enemyView = GetComponent<SecurityEnemyView>();
    }

    protected override void Start()
    {
        base.Start();
        //_enemyView.SuscribeEvents(this);
    }

    protected override void InitBehaviours()
    {
        var seek =  new Seek(_model); //for when is attacking, predicting is not good here cuz he needs to look at the player constantly, not "calculate" where he could be?
        _model.Behaviours.Add(SteeringType.Seek,seek);
        var pursuit = new Chase(_model); //for when its chasing. 
        _model.Behaviours.Add(SteeringType.Chase, pursuit);
    }

    protected override void InitFSM()
    {
        //var idle = new EnemyIdleStates<enemyStates>(_model, IdleCommand, _root );
        var patrol = new EnemyPatrolState<enemyStates>(_model, _root,SteeringType.Seek);
        var chase = new EnemyChaseState<enemyStates>(_model, _root, SteeringType.Chase);
        var shoot = new EnemyShootState<enemyStates>(_model, _root, SteeringType.Seek);
        var travelHome = new PathFindingState<enemyStates>(_model, _root, SteeringType.Seek, _model.IAStats.NearTargetRange); //TODO: rework this???

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

        travelHome.AddTransition(enemyStates.Chase, chase);
        travelHome.AddTransition(enemyStates.Shoot, shoot);
        travelHome.AddTransition(enemyStates.Patrol, patrol);

        shoot.AddTransition(enemyStates.PathFinding, travelHome);
        chase.AddTransition(enemyStates.PathFinding, travelHome);
        //patrol.AddTransition(enemyStates.PathFinding, travelHome); //en teoria si esto es volver a casa, si patruya, no lo necesitaria, ya esta en casa.

        _fsm = new FSM<enemyStates>(patrol);
    }
    protected override void InitDesitionTree() 
    {
        //INode idle = new ActionNode(() => _fsm.Transition(enemyStates.Idle));
        INode chase = new ActionNode(ChaseState);
        INode patrol = new ActionNode(PatrolState);
        INode shoot = new ActionNode(ShootState);
        INode travelHome = new ActionNode(TravelHome);

        //LOGIC: Is Player dead? -> Have I Taken Damage-> Can I See You? -> Can I Attack You?
        INode QCanShoot = new QuestionNode(() =>_model.IsInShootingRange(), shoot, chase); //if is range.... shoot, else chase. 
        INode QFarFromHome = new QuestionNode(() => _model.FarFromDestination(), travelHome, patrol); //if I am in starting pos, then patrol, else return home first
        INode QOnSight = new QuestionNode(() => _model.IsTargetInSight(), QCanShoot, QFarFromHome); //check if player is in line of sight
        INode QReceivedDamage = new QuestionNode(HasTakenDamage, chase, QOnSight); //if i have damage, then chase player, else check if I have seen him
        INode QPlayerAlive = new QuestionNode(IsPlayerDead, patrol, QReceivedDamage); //if player is not dead
        _root = QPlayerAlive;
    }

    protected void ChaseState()
    {
        isReacting = true;
        _fsm.Transition(enemyStates.Chase, showFSMTransitionInConsole);
    }
    protected void TravelHome()
    {
        isReacting = false;
        _fsm.Transition(enemyStates.PathFinding, showFSMTransitionInConsole);
    }

    protected void PatrolState()
    {
        isReacting = false;
        _fsm.Transition(enemyStates.Patrol, showFSMTransitionInConsole);
    }

    protected void ShootState()
    {
        isReacting = false;
        _fsm.Transition(enemyStates.Shoot, showFSMTransitionInConsole);
    }
}
