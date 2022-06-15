using System;
using UnityEngine;

[RequireComponent(typeof(SecurityEnemyModel))]
public class SecurityEnemyController : BaseEnemyController
{
    [SerializeField] private Transform[] wayPoints;
    private SecurityEnemyModel _model;
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
        var pursuit = new Chase(_model, _model.IAStats.PredictionTime); //for when its chasing. 
        _model.Behaviours.Add(SteeringType.Chase, pursuit);
    }

    protected override void InitFSM() //TODO: FIX FSM
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

        INode chase = new ActionNode(() => _fsm.Transition(enemyStates.Chase));
        INode patrol = new ActionNode(() => _fsm.Transition(enemyStates.Patrol));
        //INode idle = new ActionNode(() => _fsm.Transition(enemyStates.Idle));
        INode shoot = new ActionNode(() => _fsm.Transition(enemyStates.Shoot));
        INode pursuit = new ActionNode(() => _fsm.Transition(enemyStates.Chase));

        //LOGIC: Is Player dead? -> Have I Taken Damage-> Can I See You? -> Can I Attack You?
        INode QCanShoot = new QuestionNode(() =>_model.IsInShootingRange(), shoot, chase);
        INode QOnSight = new QuestionNode(() => _model.IsTargetInSight(), QCanShoot, patrol);
        INode QReceivedDamage = new QuestionNode(() => _model.HasTakenDamage, pursuit, QOnSight); //if i have damage, then pursuit player.
        INode QPlayerAlive = new QuestionNode(()=>_model.IsPlayerDead(), patrol, QReceivedDamage);
        _root = QPlayerAlive;
    }

    private void IdleCommand() //TODO FACU pondria un public virtual void Idle() en BaseEnemyModel (y en IArtificalMovement) 
    {
        _model.Move(transform.forward, 0);
    }
}
