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

        //LOGIC: Is Player dead? -> Have I Taken Damage-> Did I steal an item? -> Is there an item to steal? -> Wander if not, escape room if true.
        
        //INode QCanShoot = new QuestionNode(() => _model.IsInShootingRange(), shoot, chase); //if is range.... shoot, else chase. 
        //INode QEscapeRoom = new QuestionNode(() => _model.FarFromHome(), travelHome, patrol); //if I am in starting pos, then patrol, else return home first
        INode QItemToSteal = new QuestionNode(IsThereAnItemToSteal, travelToItem, wander); // if there is a item to steal, go for it, else wander around. 
        INode QDoIHaveAnItem = new QuestionNode(DoIHaveStolenAnItem, wander, QItemToSteal); // If I have an item... errr Wander? Else, check if there is a item to steal
        INode QReceivedDamage = new QuestionNode(HasTakenDamage, evade, QDoIHaveAnItem); //if i have damage, then evade player, else check if I I have an item already
        INode QPlayerAlive = new QuestionNode(IsPlayerDead, wander, QReceivedDamage); //if player is not dead
        _root = QPlayerAlive;
    }

    protected override void InitFSM()
    {
        var evade = new EnemyEvadeState<enemyStates>(_model, _root, SteeringType.Evade);
        var wander = new EnemyWanderState<enemyStates>(_model as IThief, _root, SteeringType.Seek, 35f); //TODO: do something for the random angle? maybe a random number?
        var travelToItem = new PathFindingState<enemyStates>(_model, _root, SteeringType.Seek, _model.IAStats.NearTargetRange); //TODO: rework this so that it has a target or something, not home range;

        travelToItem.AddTransition(enemyStates.Evade, evade);
        travelToItem.AddTransition(enemyStates.Wander, wander);

        evade.AddTransition(enemyStates.PathFinding, travelToItem);
        evade.AddTransition(enemyStates.Wander, wander);

        wander.AddTransition(enemyStates.Evade, evade);
        wander.AddTransition(enemyStates.PathFinding, travelToItem);

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
        //Debug.Log("Do I have an item already? " + (_model as IThief).ItemStolen != null);
        return (_model as IThief).ItemStolen != null;
    }

    protected bool IsThereAnItemToSteal()
    {
        //Debug.Log("Is there an item to steal " + (_model as IThief).IsThereAnItemToSteal());
        return (_model as IThief).IsThereAnItemToSteal();
    }


}
