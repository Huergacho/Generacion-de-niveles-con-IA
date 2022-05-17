using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CI_controller : MonoBehaviour
{
    CI_Model _model;
    void Start()
    {
        _model = GetComponent<CI_Model>();
    }

    void Update()
    {
        if (_model._readyToMove)
        {
            _model.Run();
        }
    }
}
