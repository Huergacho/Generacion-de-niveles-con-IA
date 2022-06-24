using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class Room : MonoBehaviour
{
    [System.Serializable]
    public class Doors
    {
        public GameObject door;
        public Vector3 dir;
    }
    [SerializeField] private List<Doors> _doors;
    [SerializeField]private List<Room> neighBours;
    [SerializeField] private Dictionary<GameObject,float> objectsToInstatiate = new Dictionary<GameObject, float>();
    [SerializeField] private List<Transform> instatiateWayPoints;
    private Dictionary<Room, Vector3> neightBoursWithDir = new Dictionary<Room, Vector3>();
    [SerializeField] private List<GameObject> instancedGameObjects;
    public List<Room> NeightBours => neighBours;

    [SerializeField] private RoomProperties properties;
    private void Start()
    {
        InstantiateRandomEntities();
        RemoveDoors();
    }
    public void InstantiateRandomEntities()
    {
        for (int i = 0; i < properties.InstanciableObjects.Count; i++)
        {
            objectsToInstatiate.Add(properties.InstanciableObjects[i].ObjectToSpawn, properties.InstanciableObjects[i].Probability);

        }
        for (int i = 0; i < instatiateWayPoints.Count; i++)
        {
            var spawnPos = instatiateWayPoints[i];
            var newObj = MyEngine.MyRandom.GetRandomWeight(objectsToInstatiate);
            if (newObj != null)
            {
                GameObject clone = Instantiate(newObj, spawnPos);
                instancedGameObjects.Add(clone);
            }
            instatiateWayPoints.RemoveAt(i);
        }
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
            if (room != null && room != this)
            {
                neighBours.Add(room);
                neightBoursWithDir.Add(room, dir);
            }

        }
    }
    public void ClearData()
    {
            neighBours.Clear();
            neightBoursWithDir.Clear();
    }
    public void ResetLevel()
    {
        for (int i = 0; i < instancedGameObjects.Count; i++)
        {
            var item = instancedGameObjects[i];
            Destroy(item);  
        }
        objectsToInstatiate.Clear();
    }
    private void RemoveDoors()
    {
        foreach (var item in neightBoursWithDir)
        {
            //if (item.Key.gameObject.activeInHierarchy)
            //{
            //    for (int i = 0; i < _doors.Count; i++)
            //    {
            //        var currDoor = _doors[i];
            //        if(_doors == item.Value)
            //        {
            //            currDoor.door.SetActive(false);
            //        }
            //    }
            //}
        }
        //for (int i = 0; i < _doors.Count; i++)
        //{
        //    var currDoor = _doors[i];
        //    if (neightBoursWithDir.ContainsValue(currDoor.dir))
        //    {
        //        print("destrui");
        //        Destroy(currDoor.door.gameObject);
        //    }
        //}
    }
}
