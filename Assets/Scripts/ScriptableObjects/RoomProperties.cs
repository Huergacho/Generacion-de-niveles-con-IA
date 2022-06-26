using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
[CreateAssetMenu(fileName = "Room", menuName = "Rooms/Default", order = 0)]
public class RoomProperties : ScriptableObject
{
    [System.Serializable]
    public class ObjectsToInstance
    {
        public float Probability => probability;
        [SerializeField] private float probability;
        public GameObject ObjectToSpawn => objectToSpawn;
        [SerializeField] private GameObject objectToSpawn;

    }

    public LayerMask RoomLayer => _roomLayer;
    [SerializeField] private LayerMask _roomLayer;
    public float DetectDistance => _detectDistance;
    [SerializeField] private float _detectDistance;

    public int LimitEnemies => _limitEnemies;
    [SerializeField] private int _limitEnemies = 2;

    public bool HardLimit => _checkHardLimit;
    [SerializeField] private bool _checkHardLimit = true;
    
    public List<ObjectsToInstance> InstanciableObjects => _instanciableObjects;
    [SerializeField] private List<ObjectsToInstance> _instanciableObjects;

    public GameObject Thief => _thiefPrefab;
    [SerializeField] private GameObject _thiefPrefab;
}
