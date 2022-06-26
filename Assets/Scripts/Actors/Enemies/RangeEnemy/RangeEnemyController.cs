using System;
using UnityEngine;

[RequireComponent(typeof(RangeEnemyModel))]
public class RangeEnemyController : BaseEnemyController
{
    protected override void InitBehaviours()
    {
        var seek =  new Seek(_model); //for when is attacking, predicting is not good here cuz he needs to look at the player constantly, not "calculate" where he could be?
        _model.Behaviours.Add(SteeringType.Seek,seek);
        var chase = new Chase(_model); //for when its chasing. 
        _model.Behaviours.Add(SteeringType.Chase, chase);
    }

    protected override void InitFSM()
    {
        var chase = new EnemyChaseState<enemyStates>(_model, _root, SteeringType.Chase);
        var shoot = new EnemyShootState<enemyStates>(_model, _root, SteeringType.Seek);
        var patrol = new EnemyPatrolState<enemyStates>(_model, _root, SteeringType.Seek);

        chase.AddTransition(enemyStates.Shoot, shoot);
        chase.AddTransition(enemyStates.Patrol, patrol);
        shoot.AddTransition(enemyStates.Chase, chase);
        shoot.AddTransition(enemyStates.Patrol, patrol);
        patrol.AddTransition(enemyStates.Shoot, shoot);
        patrol.AddTransition(enemyStates.Chase, chase);
        
        _fsm = new FSM<enemyStates>(patrol);
    }

    protected override void InitDesitionTree() 
    {
        INode chase = new ActionNode(ChaseState);
        INode patrol = new ActionNode(PatrolState);
        INode shoot = new ActionNode(ShootState);

        //LOGIC: Is Player dead? -> Have I Taken Damage-> Can I See You? -> Can I Attack You?
        INode QCanShoot = new QuestionNode(IsInShootingRange, shoot, chase); //if is range.... shoot, else chase. 
        INode QOnSight = new QuestionNode(HaveSeenTarget, QCanShoot, patrol); //check if player is in line of sight,. if not, wander
        INode QReceivedDamage = new QuestionNode(HasTakenDamage, chase, QOnSight); //if i have damage, then chase player, else check if I have seen him
        INode QPlayerAlive = new QuestionNode(IsPlayerDead, patrol, QReceivedDamage); //if player is not dead, then wander
        _root = QPlayerAlive;
    }

    protected void ChaseState()
    {
        isReacting = true;
        _fsm.Transition(enemyStates.Chase, showFSMTransitionInConsole);
    }
    protected void PatrolState()
    {
        if (showFSMTransitionInConsole)
            print("patroling state");
        isReacting = false;
        _fsm.Transition(enemyStates.Patrol, showFSMTransitionInConsole);
    }
    protected void ShootState()
    {
        if (showFSMTransitionInConsole)
            print("shooting state");
        isReacting = false;
        _fsm.Transition(enemyStates.Shoot, showFSMTransitionInConsole);
    }

    protected bool HaveSeenTarget()
    {
        var value = _model.IsTargetInSight();
        if (showFSMTransitionInConsole)
            print("Have I seen the target? " + value);
        return value;
    }

    protected bool IsInShootingRange()
    {
        var value = _model.IsInShootingRange();
        if (showFSMTransitionInConsole)
            print("Is In Shooting Range? " + value);
        return value;
    }
}
