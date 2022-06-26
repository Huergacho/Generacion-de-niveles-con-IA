using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ThiefEnemyModel))]
public class ThiefEnemyController : BaseEnemyController
{
    protected override void Awake()
    {
        base.Awake();
        if (!(_model is IThief))
            Debug.LogError("Model is not a IThief, check what´s going on on StealingEnemyController");
    }

    protected override void InitBehaviours()
    {
        var seek = new Seek(_model); //for when is wandering or going for an item
        _model.Behaviours.Add(SteeringType.Seek, seek);

        var evade = new Evade(_model); //for when it´s being chase
        _model.Behaviours.Add(SteeringType.Evade, evade);
    }

    protected override void InitDesitionTree()
    {
        INode travelToItem = new ActionNode(TravelToItem);
        INode evade = new ActionNode(Evade);
        INode wander = new ActionNode(Wander);
        INode returnToSpawnPoint = new ActionNode(TravelToEscape);

        //LOGIC: Is Player dead? -> Have I Taken Damage-> Did I steal an item? -> Is there an item to steal?
        INode QDoIHaveAnItem = new QuestionNode(DoIHaveStolenAnItem, returnToSpawnPoint, travelToItem); //If I have an item, the return to base, else go to steal one cuz there is no other reason to be here. 
        INode QReceivedDamage = new QuestionNode(HasTakenDamage, evade, QDoIHaveAnItem); //if i have damage, then chase player, else check if I have seen him
        INode QPlayerAlive = new QuestionNode(IsPlayerDead, wander, QReceivedDamage); //if player is not dead
        _root = QPlayerAlive;
    }

    protected override void InitFSM()
    {
        var evade = new EnemyEvadeState<enemyStates>(_model, _root, SteeringType.Evade);
        var wander = new EnemyWanderState<enemyStates>(_model, _root, SteeringType.Seek, _model.IAStats.RandomAngleWandering);
        var travelToDestination = new PathFindingState<enemyStates>(_model, _root, SteeringType.Seek, _model.IAStats.NearTargetRange); 

        travelToDestination.AddTransition(enemyStates.Evade, evade);
        travelToDestination.AddTransition(enemyStates.Wander, wander);

        evade.AddTransition(enemyStates.PathFinding, travelToDestination);
        evade.AddTransition(enemyStates.Wander, wander);

        wander.AddTransition(enemyStates.Evade, evade);
        wander.AddTransition(enemyStates.PathFinding, travelToDestination);

        _fsm = new FSM<enemyStates>(wander);
    }

    protected void Evade() //run from player
    {
        isReacting = true;
        _fsm.Transition(enemyStates.Evade, showFSMTransitionInConsole);
    }

    protected void TravelToItem() //Path Findind start
    {
        isReacting = false;
        _fsm.Transition(enemyStates.PathFinding, showFSMTransitionInConsole);
    }

    protected void TravelToEscape()
    {
        isReacting = false;
        (_model as IThief).ReturnHomeDestination();
        _fsm.Transition(enemyStates.PathFinding, showFSMTransitionInConsole);
    }

    protected void Wander() //wander around just in case
    {
        isReacting = false;
        _fsm.Transition(enemyStates.Wander, showFSMTransitionInConsole);
    }

    protected bool FarFromEnemy() //this should check a distance from the player to itself, if it´s far enought then true;
    {
        return _model.IsEnemyFar();
    }

    protected bool DoIHaveStolenAnItem() //if I already stole an item
    {
        return (_model as IThief).ItemStolen != null;
    }

    protected bool IsThereAnItemToSteal()
    {
        return (_model as IThief).IsThereAnItemToSteal();
    }
}
