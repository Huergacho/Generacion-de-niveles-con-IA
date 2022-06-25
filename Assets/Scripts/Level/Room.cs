﻿using System;
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
        public Room asignatedNeighBour;
    }

    [SerializeField] private List<Doors> _doors;
    [SerializeField] private List<Room> neighBours;
    [SerializeField] private Dictionary<GameObject,float> objectsToInstatiate = new Dictionary<GameObject, float>();
    [SerializeField] private List<Transform> instatiateWayPoints;
    [SerializeField] private List<GameObject> instancedGameObjects;
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private RoomProperties properties;

    private GameObject _victoryItem;
    private int randomVictory;
    private Dictionary<Room, Vector3> neightBoursWithDir = new Dictionary<Room, Vector3>();
    private int enemyCount;
    private List<IStealable> _itemsInLevel = new List<IStealable>();

    public List<IStealable> Items => _itemsInLevel;
    public List<Room> NeightBours => neighBours;
    public Transform PlayerSpawnPoint => _playerSpawnPoint;
    public bool IsEndRoom { get; set; }

    private void Awake()
    {
        InstantiateRandomEntities();
        RemoveDoors();
        
        if (IsEndRoom)
        {
            Debug.Assert(_victoryItem != null, "The victory item was never assigned");
            _victoryItem.SetActive(false);
        }

        CheckEnemyCount();    
    }

    #region Private
    private void InstantiateRandomEntities()
    {
        for (int i = 0; i < properties.InstanciableObjects.Count; i++)
        {
            objectsToInstatiate.Add(properties.InstanciableObjects[i].ObjectToSpawn, properties.InstanciableObjects[i].Probability);

        }

        for (int i = 0; i < instatiateWayPoints.Count; i++)
        {
            if (IsEndRoom && randomVictory == i) continue; //We skipd the one with the assigned victory item IF it´s the end room.

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
    
    private void RemoveDoors()
    {

        foreach (var item in _doors)
        {
            if(item.asignatedNeighBour != null && item.asignatedNeighBour.gameObject.activeInHierarchy)
            {
                    item.door.SetActive(false);
            }
        }

        //LevelManager.instance.SetCurrentLastOpenedRoom() //TODO: Add when RoomDoor is remove, move the current room in level manager to the next opened room for reference.
    }

    private void GetNeightbourd(Vector3 dir)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, properties.DetectDistance, properties.RoomLayer))
        {
            var room = hit.collider.GetComponent<Room>();
            if (room != null && room != this)
            {
                for (int i = 0; i < _doors.Count; i++)
                {
                    var currDoor = _doors[i];
                    if (currDoor.dir == dir)
                    {
                        currDoor.asignatedNeighBour = room;
                    }
                }
                neighBours.Add(room);
            }

        }
    }

    private void CheckEnemyCount()
    {
        if(enemyCount <= 0)
        {
            if (IsEndRoom)
                _victoryItem.SetActive(true);
            else
                RemoveDoors();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, properties.DetectDistance);
    }

    #endregion

    #region Public

    public void SetPlayer(GameObject player)
    {
        player.transform.position = PlayerSpawnPoint.transform.position;
    }

    public void SetVictoryItem(GameObject victoryItem)
    {
        _victoryItem = victoryItem;
        randomVictory = (int)MyEngine.MyRandom.Range(0, instatiateWayPoints.Count - 1);
        _victoryItem.transform.position = instatiateWayPoints[randomVictory].position;
    }

    public void UpdateEnemyCounter(int value)
    {
        enemyCount += value;
        CheckEnemyCount();
    }

    public void UpdateCollectableItem(IStealable item, bool isDestroyed = false)
    {
        if (!isDestroyed)
        {
            if (!_itemsInLevel.Contains(item)) //si el item no estaba ya en el listado... (cuenta los repetidos???? no deberia)
                _itemsInLevel.Add(item);
        }
        else
        {
            if (_itemsInLevel.Contains(item)) //si el item esta en el listado y fue destruido
                _itemsInLevel.Remove(item);
        }
    }

    public void GetNeightboursLinealy()
    {
        GetNeightbourd(Vector3.right);
        GetNeightbourd(Vector3.forward);
        GetNeightbourd(Vector3.left);
        GetNeightbourd(Vector3.back);
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

        IsEndRoom = false;
        objectsToInstatiate.Clear();
    }
    #endregion
}
