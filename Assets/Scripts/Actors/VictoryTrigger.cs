using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    [SerializeField] private bool isTrigger = true;

    private void Awake()
    {
        var collider = GetComponent<Collider>();
        if (collider != null)
            collider.isTrigger = isTrigger;
        else
            Debug.LogWarning($"{gameObject.name} Collider is missing!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            GameManager.instance.Victory();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            GameManager.instance.Victory();
        }
    }
}
