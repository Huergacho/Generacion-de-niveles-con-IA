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
        Shoot
    }

    protected INode _root;
    protected FSM<enemyStates> _fsm;

    protected virtual void Start()
    {
        GameManager.instance.OnPlayerInit += OnPlayerInit;
    }

    protected virtual void OnPlayerInit(PlayerModel player) 
    {
        GameManager.instance.OnPlayerInit -= OnPlayerInit;
        InitDesitionTree();
        InitFSM();
    }

    protected abstract void InitDesitionTree();
}
