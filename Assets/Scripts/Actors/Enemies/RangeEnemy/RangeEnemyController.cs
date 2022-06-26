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
    }

    protected override void Start()
    {
        base.Start();
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
        var chase = new EnemyChaseState<enemyStates>(_model, _root, SteeringType.Chase);
        var shoot = new EnemyShootState<enemyStates>(_model, _root, SteeringType.Seek);
        var wander = new EnemyWanderState<enemyStates>(_model, _root, SteeringType.Seek, _model.IAStats.RandomAngleWandering);

        chase.AddTransition(enemyStates.Shoot, shoot);
        chase.AddTransition(enemyStates.Wander, wander);
        shoot.AddTransition(enemyStates.Chase, chase);
        shoot.AddTransition(enemyStates.Wander, wander);
        wander.AddTransition(enemyStates.Chase, chase);
        
        _fsm = new FSM<enemyStates>(wander);
    }
    protected override void InitDesitionTree() 
    {
        INode chase = new ActionNode(ChaseState);
        INode patrol = new ActionNode(PatrolState);
        INode shoot = new ActionNode(ShootState);
        INode travelHome = new ActionNode(TravelHome);
        INode wander = new ActionNode(Wander);

        //LOGIC: Is Player dead? -> Have I Taken Damage-> Can I See You? -> Can I Attack You?
        INode QCanShoot = new QuestionNode(() =>_model.IsInShootingRange(), shoot, chase); //if is range.... shoot, else chase. 
        INode QOnSight = new QuestionNode(() => _model.IsTargetInSight(), QCanShoot, wander); //check if player is in line of sight,. if not, wander
        INode QReceivedDamage = new QuestionNode(HasTakenDamage, chase, QOnSight); //if i have damage, then chase player, else check if I have seen him
        INode QPlayerAlive = new QuestionNode(IsPlayerDead, wander, QReceivedDamage); //if player is not dead, then wander
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

    protected void Wander() //wander around just in case
    {
        isReacting = false;
        _fsm.Transition(enemyStates.Wander, showFSMTransitionInConsole);
    }
}
