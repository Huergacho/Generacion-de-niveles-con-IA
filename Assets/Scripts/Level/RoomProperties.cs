using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
[CreateAssetMenu(fileName = "Room", menuName = "Rooms/Default",order = 0)]
public class RoomProperties : ScriptableObject
{
    [SerializeField] private List<GameObject> _presets;
    public List<GameObject> Presets => _presets;
    [SerializeField] private LayerMask _roomLayer;
    public LayerMask RoomLayer => _roomLayer;
    [SerializeField] private float _detectDistance;
    public float DetectDistance => _detectDistance;
}
