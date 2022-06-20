using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RangeEnemyView : MonoBehaviour
{
    private Animator _animator;
    private RangeEnemyModel _model;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _model = GetComponent<RangeEnemyModel>();
        _model.OnDetect += DetectAnimation;
    }
    void DetectAnimation(bool detectBool)
    {
        _animator.SetBool("Detect",detectBool);
    }

    private void OnDestroy()
    {
        _model.OnDetect -= DetectAnimation;
    }
}
