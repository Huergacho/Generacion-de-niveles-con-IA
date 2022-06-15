using System;
using UnityEngine;
using System.Collections.Generic;

class PlayerRunState<T> : State<T>
{
    private T _walkInput;
    private T _runInput;
    private T _idleInput;
    private float _desiredSpeed;

    private PlayerInputs _playerInputs;

    private Action _onShoot;
    private Action<Vector3, float> _onRun;
    private Action _animation;

    public PlayerRunState(T walkInput, T runInput, T idleInput, Action<Vector3, float> onRun, Action onShoot, float desiredSpeed, Action animation)
    {
        _walkInput = walkInput;
        _runInput = runInput;
        _idleInput = idleInput;
        _onRun = onRun;
        _onShoot = onShoot;
        _desiredSpeed = desiredSpeed;
        _animation = animation;
    }

    public override void Awake()
    {
        //Nos suscribimos a los eventos
        GameManager.instance.InputManager.OnAttack += OnShoot;
        GameManager.instance.InputManager.OnMove += OnMove;
        GameManager.instance.InputManager.OnShiftSpeed += OnWalk;
    }

    public override void Execute()
    {
        GameManager.instance.InputManager.PlayerUpdate(); //Checkeo de inputs
    }


    private void OnMove(Vector3 movement)
    {
        if (movement == Vector3.zero) //if there is no movement... 
        {
            _parentFSM.Transition(_idleInput);
            return;
        }

        _onRun?.Invoke(new Vector3(movement.x, 0,movement.z), _desiredSpeed);
        _animation?.Invoke();
    }

    private void OnWalk(bool isRunning)
    {
        if(!isRunning)
        _parentFSM.Transition(_walkInput);
    }

    private void OnShoot()
    {
        _onShoot?.Invoke();
    }

    public override void Sleep()
    {
        GameManager.instance.InputManager.OnAttack -= OnShoot;
        GameManager.instance.InputManager.OnMove -= OnMove;
        GameManager.instance.InputManager.OnShiftSpeed -= OnWalk;
    }
}
