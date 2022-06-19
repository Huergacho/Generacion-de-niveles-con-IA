using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class PlayerWalkStates<T> : State<T>
{
    private T _idleInput;
    private T _runInput;
    private Action<Vector2> _onWalk;
    private PlayerInputs _playerInputs;
    private bool _showFSMTransitionInConsole;

    public PlayerWalkStates(T idleInput, T runInput, Action<Vector2> onWalk, PlayerInputs playerInputs, bool showFSMTransitionInConsole)
    {
        _idleInput = idleInput;
        _runInput = runInput;
        _onWalk = onWalk;
        _playerInputs = playerInputs;
        _showFSMTransitionInConsole = showFSMTransitionInConsole;
    }
    public override void Execute()
    {
        _playerInputs.UpdateInputs();

        if (!_playerInputs.IsMoving())
        {
            _parentFSM.Transition(_idleInput, _showFSMTransitionInConsole);
            return;
        }
        if (_playerInputs.IsRunning())
        {
            _parentFSM.Transition(_runInput, _showFSMTransitionInConsole);
            return;
        }
        _onWalk?.Invoke(new Vector3(_playerInputs.GetH,0,_playerInputs.GetV));
    }
}

