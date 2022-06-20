using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : EntityModel, ITarget
{
    [SerializeField] private Transform _firePoint;

    private LazerGun _gun;
    private Camera _camera;

    //Propierties
    public float GetVel => _rb.velocity.magnitude;
    public Vector3 GetFoward => transform.forward;

    #region UnityMethods
    protected override void Awake()
    { 
        base.Awake();
        _camera = Camera.main;
        _gun = GetComponent<LazerGun>();
        _gun.SetGunCD(_actorStats.ShootCooldown);
    }

    private void Start()
    {
        GameManager.instance.SetPlayer(this);
    }
    #endregion

    #region MouseCalculation
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
                return target; //TODO: Facuu que onda esto???
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
    
    public void SuscribeEvents(PlayerController controller) // TODO: Facu esto esta mal porque o guardo la referencia del controller, o cuando me destruyo, no hago en el OnDestroy la desuscripcion del player controller
    {
        controller._onShoot += Shoot;
        controller._onMove += Move;
    }

    public override void Die()
    {
        GameManager.instance.GameOver();
    }

    public void SmoothRotation(Vector3 dest)
    {
        var direction = (dest - transform.position);
        if (direction != Vector3.zero)
        {
            var rotDestiny = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotDestiny, ActorStats.RotSpeed * Time.deltaTime);
        }
    }
}
