using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class RoomNode : MonoBehaviour
{
    [SerializeField] private RoomNode[] neightBours;
    public RoomNode[] GetNeightbourd()
    {
        return neightBours;
    }
}
