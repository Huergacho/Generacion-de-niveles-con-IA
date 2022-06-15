using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class PlayerWalkState<T> : State<T>
{
    private T _idleInput;
    private T _runInput;
    private float _desiredSpeed;

    private Action<Vector3, float> _onWalk;
    private Action _onShoot;
    private Action _animation;

    public PlayerWalkState(T idleInput, T runInput, Action<Vector3,float> onWalk, Action onShoot, float desiredSpeed, Action animation)
    {
        _idleInput = idleInput;
        _runInput = runInput;
        _onWalk = onWalk;
        _onShoot = onShoot;
        _desiredSpeed = desiredSpeed;
        _animation = animation;
    }

    public override void Execute()
    {
        GameManager.instance.InputManager.PlayerUpdate();
    }

    public override void Awake()
    {
        //Nos suscribimos a los eventos
        GameManager.instance.InputManager.OnMove += OnMove;
        GameManager.instance.InputManager.OnAttack += OnShoot;
        GameManager.instance.InputManager.OnShiftSpeed += OnRun;
    }

    private void OnMove(Vector3 movement)
    {
        if(movement == Vector3.zero) // //if there is no movement... 
        {
            _parentFSM.Transition(_idleInput);
            return;
        }

        _onWalk?.Invoke(new Vector3(movement.x, 0,movement.z), _desiredSpeed);
        _animation?.Invoke();
    }

    private void OnRun(bool isRunning)
    {
        if(isRunning)
            _parentFSM.Transition(_runInput);
    }

    private void OnShoot()
    {
        _onShoot?.Invoke();
    }

    public override void Sleep()
    {
        GameManager.instance.InputManager.OnMove -= OnMove;
        GameManager.instance.InputManager.OnAttack -= OnShoot;
        GameManager.instance.InputManager.OnShiftSpeed -= OnRun;
    }
}

