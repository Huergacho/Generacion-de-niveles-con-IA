﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class PlayerIdleState<T> : State<T>
{
    private T _walkInput;
    private Action<Vector3, float> _onIdle;
    private Action _onShoot;
    private Action _animation;
    private bool _showFSMTransitionInConsole;

    public PlayerIdleState(T walkInput, Action<Vector3,float> onIdle, Action onShoot, Action animation, bool showFSMTransitionInConsole)
    {
        _walkInput = walkInput;
        _onIdle = onIdle;
        _onShoot = onShoot;
        _animation = animation;
        _showFSMTransitionInConsole = showFSMTransitionInConsole;
    }

    public override void Awake()
    {
        //Nos suscribimos a los eventos
        GameManager.instance.InputManager.OnMove += OnMove;
        GameManager.instance.InputManager.OnAttack += OnShoot;
    }

    public override void Execute()
    {
        GameManager.instance.InputManager.PlayerUpdate(); //Checkeo de inputs
    }

    private void OnMove(Vector3 move)
    {
        if(move != Vector3.zero) //if it´s moving....
        {
            _parentFSM.Transition(_walkInput, _showFSMTransitionInConsole);
            return;
        }

        _onIdle?.Invoke(new Vector2(move.x, move.z), 0); //TODO: ...facu porque invoca algo aca? digoo.. esta quieto. 
        _animation?.Invoke();
    }

    private void OnShoot()
    {
        _onShoot?.Invoke();
    }

    public override void Sleep()
    {
        GameManager.instance.InputManager.OnAttack -= OnShoot;
        GameManager.instance.InputManager.OnMove -= OnMove;
    }
}
