using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolRoute : MonoBehaviour
{
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] bool AddComponentContainer;
    private GameObject[] patrolNodes;
    //public Transform[] PatrolPoints => patrolPoints;
    public GameObject[] PatrolNodes => patrolNodes;

    public void Initialize() //Es un initialize y no en start para que el dueño lo tenga cuando lo necesite y no haya lios con el orden de los Starts/Awake
    {
        if (patrolPoints.Length < 1)
            Debug.LogError("PatrolNodes shouldn't be empty");

        ConvertToArray();
        ConvertToNodes();
    }

    private void ConvertToArray() //Toma del gameobject todos los transforms de los hijos y los pone en un array de transforms. Incluye el spawn point como transform. 
    {
        if (AddComponentContainer)
        {
            patrolPoints = new Transform[this.transform.childCount + 1];
            patrolPoints[0] = this.transform; //Tomamos como 0 el punto de partida.
            for (int i = 0; i < this.transform.childCount; i++)
            {
                patrolPoints[i + 1] = this.transform.GetChild(i).GetComponent<Transform>();
            }
        }
        else
        {
            patrolPoints = new Transform[this.transform.childCount];
            for (int i = 0; i < this.transform.childCount; i++)
            {
                patrolPoints[i] = this.transform.GetChild(i).GetComponent<Transform>();
            }
        }
    }

    private void ConvertToNodes() //Y aca los instancia a objetos para que no se muevan con el enemigo. 
    {
        patrolNodes = new GameObject[patrolPoints.Length];

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            GameObject aux = new GameObject("PatrolNode " + i);
            aux.transform.position = patrolPoints[i].transform.position;
            if (LevelManager.instance.PatrolNodeParent != null) //Si el levelmanager tiene asignado un gameobject para tirarle los patrol nodes adentro
                aux.transform.parent = LevelManager.instance.PatrolNodeParent; //asignaselos como padre
            patrolNodes[i] = aux;
        }

    }

    public void DestroyPatrolNodes() //Cuando se destruye, destruye los nodos tambien. 
    {
        for (int i = patrolNodes.Length - 1; i >= 0; i--)
        {
            Destroy(patrolNodes[i]);
        }
    }
}
