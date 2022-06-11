using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region KeyCodes
    [SerializeField] private KeyCode attack = KeyCode.Mouse0;
    [SerializeField] private KeyCode pause = KeyCode.Escape;
    
    private string horizontalAxis = "Horizontal";
    private string verticalAxis = "Vertical";
    #endregion

    public bool IsMoving { get; private set; }

    #region Events
    public Action OnPause;
    public Action OnAttack;
    public Action<Vector3> OnMove;
    #endregion

    #region Unity
    void Update()
    {
        CheckPause();
    }

    public void PlayerUpdate()
    {
        if (!GameManager.instance.IsGamePaused)
        {
            CheckAttack();
            CheckMovement();
        }
    }
    #endregion

    #region Private
    private void CheckMovement() //Moverlo para controllar en idle (chequeo si no me muevo) y en move (si me muevo) del player. 
    {
        float horizontal = Input.GetAxisRaw(horizontalAxis);
        float vertical = Input.GetAxisRaw(verticalAxis);
        IsMoving = (vertical != 0 || horizontal != 0) ? true : false;
        OnMove?.Invoke(new Vector3(horizontal, 0, vertical));
    }
    private void CheckAttack()
    {
        if (Input.GetKeyDown(attack))
            OnAttack?.Invoke();
    }

    private void CheckPause()
    {
        if(Input.GetKeyDown(pause))
            GameManager.instance.Pause(!GameManager.instance.IsGamePaused);
    }
    #endregion
}
