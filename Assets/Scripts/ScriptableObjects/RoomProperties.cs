using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
[CreateAssetMenu(fileName = "Room", menuName = "Rooms/Default", order = 0)]
public class RoomProperties : ScriptableObject
{
    [SerializeField] private GameObject[] _presets;
    public GameObject[] Presets => _presets;
    [SerializeField] private LayerMask _roomLayer;
    public LayerMask RoomLayer => _roomLayer;
    [SerializeField] private float _detectDistance;
    public float DetectDistance => _detectDistance;

    [System.Serializable]
    public class ObjectsToInstance
    {
        [SerializeField] private float probability;
        public float Probability => probability;
        [SerializeField] private GameObject objectToSpawn;
        public GameObject ObjectToSpawn => objectToSpawn;

    }
    [SerializeField] private List<ObjectsToInstance> _instanciableObjects;
    public List<ObjectsToInstance> InstanciableObjects => _instanciableObjects;

    [SerializeField] private GameObject _thief;
    public GameObject Thief => _thief;

}
