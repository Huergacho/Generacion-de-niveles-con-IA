using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class PlayerModel : MonoBehaviour,IVel
{
    [SerializeField] private ActorStats _actorStats;
    [SerializeField] private Transform _firePoint;

    private LazerGun _gun;
    private Camera _camera;
    private Rigidbody _rb;

    //Propierties
    public ActorStats ActorStats => _actorStats;
    public LifeController LifeController { get; private set; }
    public float GetVel => _rb.velocity.magnitude;
    public Vector3 GetFoward => _rb.velocity.normalized;
    public Transform GetTarget => transform;

    #region UnityMethods
    private void Awake()
    {
        _camera = Camera.main;
        _gun = GetComponent<LazerGun>();
        _rb = GetComponent<Rigidbody>();
        LifeController = GetComponent<LifeController>();
        LifeController.SetMaxLife(_actorStats.MaxLife);
        LifeController.OnDie += OnDie;
    }

    private void Start()
    {
        GameManager.instance.SetPlayer(this);
    }
    #endregion

    #region Movement
    public void Move(Vector2 dir, float desiredSpeed)
    {
        _rb.velocity = new Vector3(desiredSpeed * dir.normalized.x, _rb.velocity.y, desiredSpeed * dir.normalized.y);
        SmoothRotation(GetMousePosition());
    }
    #endregion

    #region MouseCalculation
    private void SmoothRotation(Vector3 dest)
    {
        var direction = (dest - transform.position);
        if (direction != Vector3.zero)
        {
            var rotDestiny = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotDestiny, ActorStats.RotSpeed * Time.deltaTime);
        }
    }
    private Vector3 GetMousePosition()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            var target = hitInfo.point;
            target.y = transform.position.y;
            var distance = Vector3.Distance(transform.position, hitInfo.point);
            if (distance >= 1f)
            {
                return target;
            }
            else
            {
                return target;
            }
        }
        else
        {
            return Vector3.zero;
        }
    }
    public void Shoot()
    {
        _gun.Shoot(_firePoint.position,(GetMousePosition() - new Vector3(_firePoint.position.x,0,_firePoint.position.z)).normalized);
    }
    #endregion
    
    public void SuscribeEvents(PlayerController controller)
    {

        controller._onShoot += Shoot;
        controller._onMove += Move;
    }

    public void OnDie()
    {
        GameManager.instance.PlayerIsDead();
    }
}
