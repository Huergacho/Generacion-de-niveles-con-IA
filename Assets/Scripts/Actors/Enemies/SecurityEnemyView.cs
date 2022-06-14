using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SecurityEnemyView : MonoBehaviour
{
    private Animator _animator;
    private SecurityEnemyModel _model;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _model = GetComponent<SecurityEnemyModel>();
        _model.onDetect += DetectAnimation;
    }
    void DetectAnimation(bool detectBool)
    {
        _animator.SetBool("Detect",detectBool);
    }

    private void OnDestroy()
    {
        _model.onDetect -= DetectAnimation;
    }
}
