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
        INode returnToSpawnPoint = new ActionNode(TravelToEscape);

        //LOGIC: Is Player dead? -> Have I Taken Damage-> Did I steal an item? -> Is there an item to steal?
        //TODO: it´s still missing what it should do if it returns to spawn point (whether he stole or not a thing, should he dissapear?)
        INode QIsThereAnItemToSteal = new QuestionNode(IsThereAnItemToSteal, travelToItem, returnToSpawnPoint); //if there is no item to steal, return to base, else go steal it
        INode QDoIHaveAnItem = new QuestionNode(DoIHaveStolenAnItem, returnToSpawnPoint, QIsThereAnItemToSteal); //If I have an item, the return to base, else go to steal one cuz there is no other reason to be here. 
        INode QFarFromEnemy = new QuestionNode(FarFromEnemy, QDoIHaveAnItem, evade); //if I´m far, then go check if I stole something, else evade
        INode QReceivedDamage = new QuestionNode(HasTakenDamage, QFarFromEnemy, QDoIHaveAnItem); //if i have damage, then check if I´m far from player, else check if there is a player
        INode QPlayerAlive = new QuestionNode(IsPlayerDead, QDoIHaveAnItem, QReceivedDamage); //if player is not dead
        _root = QPlayerAlive;
    }

    protected override void InitFSM()
    {
        var evade = new EnemyEvadeState<enemyStates>(_model, _root, SteeringType.Evade);
        var seek = new EnemySeekState<enemyStates>(_model, _root, SteeringType.Seek);
        var travelToDestination = new PathFindingState<enemyStates>(_model, _root, SteeringType.Seek, _model.IAStats.NearTargetRange);

        travelToDestination.AddTransition(enemyStates.Evade, evade);
        travelToDestination.AddTransition(enemyStates.Seek, seek);
        evade.AddTransition(enemyStates.PathFinding, travelToDestination);
        evade.AddTransition(enemyStates.Seek, seek);
        seek.AddTransition(enemyStates.PathFinding, travelToDestination);
        seek.AddTransition(enemyStates.Evade, evade);

        _fsm = new FSM<enemyStates>(seek);
    }

    protected void Evade() //run from player
    {
        isReacting = true;
        _fsm.Transition(enemyStates.Evade, showFSMTransitionInConsole);
    }

    protected void TravelToItem() //Path Findind To Coin
    {
        isReacting = false;
        (_model as IThief).IsThereAnItemToSteal();
        _fsm.Transition(enemyStates.Seek, showFSMTransitionInConsole);
    }

    protected void TravelToEscape() //Path Findind to starting point;
    {
        isReacting = false;
        (_model as IThief).ReturnHomeDestination();
        _fsm.Transition(enemyStates.PathFinding, showFSMTransitionInConsole);
    }

    protected bool FarFromEnemy() //this should check a distance from the player to itself, if it´s far enought then true;
    {
        var value = _model.IsEnemyFar();
        if (showFSMTransitionInConsole)
            print("FarFromEnemy? " + value);
        return value;
    }

    protected bool DoIHaveStolenAnItem() //if I already stole an item
    {
        var value = (_model as IThief).ItemStolen != null;
        if (showFSMTransitionInConsole)
            print("Did I steal an item? " + value);
        return value;
    }

    protected bool IsThereAnItemToSteal()
    {
        var value = (_model as IThief).IsThereAnItemToSteal();
        if (showFSMTransitionInConsole)
            print("Is there an item to steal? " + value);
        return value;
    }
}
