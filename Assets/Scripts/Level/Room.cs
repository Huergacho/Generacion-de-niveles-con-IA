using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class Room : MonoBehaviour
{
    [SerializeField] private List<Room> neighBours;
    public List<Room> NeightBours => neighBours;

    [SerializeField] private LayerMask roomLayer;
    [SerializeField] private float detectDistance;
    public void GetNeightBoursRadially()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, detectDistance,roomLayer);
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
        Gizmos.DrawWireSphere(transform.position, detectDistance);
    }
    void GetNeightbourd(Vector3 dir)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, detectDistance,roomLayer))
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
