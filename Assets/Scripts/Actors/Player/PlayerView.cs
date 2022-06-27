using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerView : MonoBehaviour
{
    private Animator _animator;
    private float speedValue;
    [SerializeField] private float transitionTime;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SuscribeEvents(PlayerController controller)
    {
        controller._onShoot += Shoot;
    }

    public void Move()
    {
        //_animator.SetFloat("Speed", 1);
        //_animator.SetFloat("Speed", Mathf.SmoothStep(_animator.GetFloat("Speed"), 1f, Time.deltaTime * transitionTime));
    }

   public void Idle()
    {
        //_animator.SetFloat("Speed", 0);
        //if(_animator.GetFloat("Speed") < 0.1f)
        //{
        //    _animator.SetFloat("Speed", 0);

        //}
        //_animator.SetFloat("Speed", Mathf.SmoothStep(_animator.GetFloat("Speed"), 0f, Time.deltaTime * transitionTime));

    }

    void Shoot()
    {
        //_animator.Play("Shoot");
    }
}
