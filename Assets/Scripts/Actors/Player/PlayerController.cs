using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[RequireComponent (typeof (PlayerModel), typeof (PlayerInputs))]
public class PlayerController : MonoBehaviour
{
    enum PlayerHelper
    {
        Idle,
        Walk,
        Run
    }

    private PlayerModel _playerModel;
    private PlayerView _playerView;
    private FSM<PlayerHelper> _fsm;

    //EVENTS
    public event Action _onIdle;
    public event Action<Vector2,float> _onMove;
    public event Action _onShoot;
    public event Action _onDie;

    private void Awake()
    {
        _playerModel = GetComponent<PlayerModel>();
        _playerView = GetComponent<PlayerView>();
        InitFSM();
    }

    private void Start()
    {
        _playerModel.LifeController.OnDie = DieActions;
        _playerModel.SuscribeEvents(this);
        _playerView.SuscribeEvents(this);
    }

    private void InitFSM()
    {
        var idle = new PlayerIdleState<PlayerHelper>(PlayerHelper.Walk, MovementCommand, ShootCommand, _playerView.Idle);
        var walk = new PlayerWalkState<PlayerHelper>(PlayerHelper.Idle, PlayerHelper.Run, MovementCommand, ShootCommand, _playerModel.ActorStats.WalkSpeed,_playerView.Move);
        var run = new PlayerRunState<PlayerHelper>(PlayerHelper.Walk,PlayerHelper.Run, PlayerHelper.Idle, MovementCommand, ShootCommand, _playerModel.ActorStats.RunSpeed,_playerView.Move);

        idle.AddTransition(PlayerHelper.Walk, walk);
        idle.AddTransition(PlayerHelper.Run, run);

        walk.AddTransition(PlayerHelper.Idle, idle);
        walk.AddTransition(PlayerHelper.Run, run);

        run.AddTransition(PlayerHelper.Walk, walk);
        run.AddTransition(PlayerHelper.Idle, idle);

        _fsm = new FSM<PlayerHelper>(idle);
    }

    private void Update()
    {
        _fsm.UpdateState();
    }

    private void MovementCommand(Vector2 dir, float desiredSpeed)
    {
        _onMove?.Invoke(dir, desiredSpeed);
    }

    private void ShootCommand()
    {
        _onShoot?.Invoke();
    }

    private void DieActions()
    {
        _onDie?.Invoke();
        GameManager.instance.PlayerIsDead();
    }
}
