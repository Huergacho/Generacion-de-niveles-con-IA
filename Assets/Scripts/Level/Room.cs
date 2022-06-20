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
    public List<Room> NeightBours => neighBours;

    [SerializeField] private RoomProperties properties;
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

    public void GetRandomized()
    {

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
