using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class Room : MonoBehaviour
{

    [SerializeField]private List<Room> neighBours;
    [SerializeField] private Dictionary<GameObject,float> objectsToInstatiate = new Dictionary<GameObject, float>();
    [SerializeField] private List<Transform> instatiateWayPoints;
    public List<Room> NeightBours => neighBours;

    [SerializeField] private RoomProperties properties;
    //[SerializeField] private GameObject currentPreset; // Esto queda por si en algun momento queremos volver a meter lo de los presets :D
    private void Start()
    {
        for (int i = 0; i < properties.InstanciableObjects.Count; i++)
        {
            objectsToInstatiate.Add(properties.InstanciableObjects[i].ObjectToSpawn, properties.InstanciableObjects[i].Probability);

        }
    }
    public void GetNeightBoursRadially()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, properties.DetectDistance, properties.RoomLayer);
        for (int i = 0; i < colls.Length; i++)
        {
            var curr = colls[i];
            Room currentRoom = curr.gameObject.GetComponent<Room>();
            if(currentRoom != null && currentRoom != this)
            { 
              neighBours.Add(currentRoom);
            }
        }   
    }
    public void InstantiateRandomEntities()
    {

        for (int i = 0; i < instatiateWayPoints.Count; i++)
        {
            var spawnPos = instatiateWayPoints[i];
            var newObj = MyEngine.MyRandom.GetRandomWeight(objectsToInstatiate);
            Instantiate(newObj, spawnPos);
            instatiateWayPoints.RemoveAt(i);
        }
    }
    public void GetRandomized()
    {
        //var currRoom = UnityEngine.Random.Range(0, properties.Presets.Length);     // Esto queda por si en algun momento queremos volver a meter lo de los presets :D
        //if(currentPreset != null)
        //{
        //    Destroy(currentPreset);
        //    currentPreset = null;
        //}
        //currentPreset = properties.Presets[currRoom];
        //Instantiate(currentPreset, transform);

        InstantiateRandomEntities();
    }
    public void GetNeightboursLinealy()
    {
        GetNeightbourd(Vector3.right);
        GetNeightbourd(Vector3.forward);
        GetNeightbourd(Vector3.left);
        GetNeightbourd(Vector3.back);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, properties.DetectDistance);
    }
    void GetNeightbourd(Vector3 dir)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, properties.DetectDistance, properties.RoomLayer))
        {
            var room = hit.collider.GetComponent<Room>();
            if(room != null && room != this)
            neighBours.Add(room);
        }
    }
    public void ClearData()
    {
        for (int i = 0; i < neighBours.Count; i++)
        {
            neighBours.RemoveAt(i);
        }
    }
}
