using System;
using UnityEngine;

[RequireComponent(typeof(SecurityEnemyModel))]
public class SecurityEnemyController : BaseEnemyController
{
    [SerializeField] private Transform[] wayPoints;
    private SecurityEnemyModel _model;
    private bool isChasing;
    //private SecurityEnemyView _enemyView;

    protected override void Awake()
    {
        base.Awake();
        _model = GetComponent<SecurityEnemyModel>();
        //_enemyView = GetComponent<SecurityEnemyView>();
    }

    protected void Start()
    {
        //_enemyView.SuscribeEvents(this);
        InitBehaviours();
        InitDesitionTree();
        InitFSM();
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
        //INode idle = new ActionNode(() => _fsm.Transition(enemyStates.Idle));
        INode chase = new ActionNode(ChaseState);
        INode patrol = new ActionNode(PatrolState);
        INode shoot = new ActionNode(ShootState);

        //LOGIC: Is Player dead? -> Have I Taken Damage-> Can I See You? -> Can I Attack You?
        INode QCanShoot = new QuestionNode(() =>_model.IsInShootingRange(), shoot, chase); //if is range.... shoot, else chase. 
        INode QOnSight = new QuestionNode(() => _model.IsTargetInSight(), QCanShoot, patrol); //check if player is in line of sight
        INode QReceivedDamage = new QuestionNode(HasTakenDamage, chase, QOnSight); //if i have damage, then chase player, else check if I have seen him
        INode QPlayerAlive = new QuestionNode(IsPlayerDead, patrol, QReceivedDamage); //if player is not dead
        _root = QPlayerAlive;
    } 

    protected void ChaseState()
    {
        isChasing = true;
        _fsm.Transition(enemyStates.Chase, showFSMTransitionInConsole);
    }

    protected void PatrolState()
    {
        isChasing = false;
        _fsm.Transition(enemyStates.Patrol, showFSMTransitionInConsole);
    }

    protected void ShootState()
    {
        isChasing = false;
        _fsm.Transition(enemyStates.Shoot, showFSMTransitionInConsole);
    }

    private bool IsPlayerDead()
    {
        //Debug.Log("Player is Dead " + _model.IsPlayerDead());
        return _model.IsPlayerDead();
    }

    private bool HasTakenDamage() //If I have receive damage and I'm not doing something about it then... return true
    {
        //Debug.Log("Do I have damage " + (_model.HasTakenDamage && !isChasing));
        return _model.HasTakenDamage && !isChasing;
    }

    //private void IdleCommand() //TODO FACU fijate si esto se va a usar o no? pondria un public virtual void Idle() en BaseEnemyModel (y en IArtificalMovement) 
    //{
    //    _model.Move(transform.forward, 0);
    //}
}
