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

    protected virtual void Awake()
    {
        //_fsm = new FSM<enemyStates>(); //Facu hizo que la FSM cuando se crea si o si tiene que tener un state asignado en el constructor.
    }

    protected abstract void InitBehaviours();
    protected abstract void InitDesitionTree();

    protected virtual void Update()
    {
        if(!GameManager.instance.IsGamePaused)
            _fsm?.UpdateState();
    }
}
