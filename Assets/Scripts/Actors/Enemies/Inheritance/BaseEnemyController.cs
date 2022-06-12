using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseEnemyController : MonoBehaviour
{
    protected INode _root;

    protected virtual void Start()
    {
        InitDesitionTree();
        InitFSM();
    }

    protected abstract void InitDesitionTree();

    protected abstract void InitFSM();

}
