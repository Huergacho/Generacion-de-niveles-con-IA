using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMaskMovement : MonoBehaviour
{
    [SerializeField] private Material _mat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _mat.SetVector("_PointPosition", transform.position);  
    }

}
