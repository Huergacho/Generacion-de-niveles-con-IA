using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[RequireComponent (typeof (PlayerModel))]
public class PlayerController : EntityController
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

    private void Awake()
    {
        _playerModel = GetComponent<PlayerModel>();
        _playerView = GetComponent<PlayerView>();
    }

    private void Start()
    {
        InitFSM();
        _playerModel.SuscribeEvents(this);
        _playerView.SuscribeEvents(this);
    }

    protected override void InitFSM()
    {
        var idle = new PlayerIdleState<PlayerHelper>(PlayerHelper.Walk, Move, Shoot, _playerView.Idle, showFSMTransitionInConsole);
        var walk = new PlayerWalkState<PlayerHelper>(PlayerHelper.Idle, PlayerHelper.Run, Move, Shoot, _playerModel.ActorStats.WalkSpeed,_playerView.Move, showFSMTransitionInConsole);
        var run = new PlayerRunState<PlayerHelper>(PlayerHelper.Walk,PlayerHelper.Run, PlayerHelper.Idle, Move, Shoot, _playerModel.ActorStats.RunSpeed,_playerView.Move, showFSMTransitionInConsole);

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
        if(!GameManager.instance.IsGamePaused)
            _fsm.UpdateState();
    }
}
