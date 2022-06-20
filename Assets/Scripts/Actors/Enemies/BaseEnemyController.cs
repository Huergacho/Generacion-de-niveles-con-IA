using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseEnemyController : EntityController
{
    protected enum enemyStates
    {
        Idle,
        Patrol,
        Seek,
        Chase,
        Shoot,
        PathFinding,
        Wander,
        Evade
    }

    protected INode _root;
    protected FSM<enemyStates> _fsm;
    protected BaseEnemyModel _model;
    protected bool isReacting;

    protected virtual void Awake()
    {
        _model = GetComponent<BaseEnemyModel>();
    }

    protected virtual void Start()
    {
        InitBehaviours();
        InitDesitionTree();
        InitFSM();
    }

    protected abstract void InitBehaviours();
    protected abstract void InitDesitionTree();

    protected virtual void Update()
    {
        if(!GameManager.instance.IsGamePaused)
            _fsm?.UpdateState();
    }

    protected bool IsPlayerDead()
    {
        //Debug.Log("Player is Dead " + _model.IsPlayerDead());
        return _model.IsPlayerDead();
    }

    protected bool HasTakenDamage() //If I have receive damage and I'm not doing something about it then... return true
    {
        //Debug.Log("Do I have damage " + (_model.HasTakenDamage && !isChasing));
        return _model.HasTakenDamage && !isReacting;
    }

    //private void IdleCommand() //TODO FACU fijate si esto se va a usar o no? pondria un public virtual void Idle()  en IArtificalMovement
    //{
    //    _model.Move(transform.forward, 0);
    //}
}
