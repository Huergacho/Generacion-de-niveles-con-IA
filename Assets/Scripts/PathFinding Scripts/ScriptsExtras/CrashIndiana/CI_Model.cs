using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CI_Model : MonoBehaviour, IPoints
{
    public List<Vector3> waypoints;
    public float speed = 2;
    public float speedRot = 10;
    public bool _readyToMove;
    int _nextPoint = 0;
    Animator _anim;
    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }
    public void Move(Vector3 dir)
    {
        dir.y = 0;
        transform.position += Time.deltaTime * dir * speed; ;
        transform.forward = Vector3.Lerp(transform.forward, dir, speedRot * Time.deltaTime);
        _anim.SetFloat("Vel", 1);
    }
    public void SetWayPoints(List<Node> newPoints)
    {
        var list = new List<Vector3>();
        for (int i = 0; i < newPoints.Count; i++)
        {
            list.Add(newPoints[i].transform.position);
        }
        SetWayPoints(list);
    }
    public void SetWayPoints(List<Vector3> newPoints)
    {
        _nextPoint = 0;
        if (newPoints.Count == 0) return;
        _anim.Play("CIA_Idle");
        waypoints = newPoints;
        var pos = waypoints[_nextPoint];
        pos.y = transform.position.y;
        transform.position = pos;
        _readyToMove = true;
    }
    public void Run()
    {
        var point = waypoints[_nextPoint];
        var posPoint = point;
        posPoint.y = transform.position.y;
        Vector3 dir = posPoint - transform.position;
        if (dir.magnitude < 0.2f)
        {
            if (_nextPoint + 1 < waypoints.Count)
                _nextPoint++;
            else
            {
                _readyToMove = false;
                _anim.SetTrigger("Finish");
                _anim.SetFloat("Vel", 0);
                return;
            }
        }
        Move(dir.normalized);
    }
}
