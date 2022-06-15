using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class LazerGun : MonoBehaviour
{
    [SerializeField] private BulletStats _bulletStats;
    [SerializeField] private GunStats _gunStats;
    private float currentTime;

    private void Awake()
    {
        currentTime = _gunStats.ShootCooldown; //TODO fix null warning?
    }

    private void Update()
    {
        if (GameManager.instance.IsGamePaused) return;
        if(currentTime < _gunStats.ShootCooldown)
            currentTime += Time.deltaTime;
    }

    public void Shoot(Vector3 startPos, Vector3 dir)
    {
        if (currentTime < _gunStats.ShootCooldown) return;

        if (Physics.Raycast(startPos,dir, out RaycastHit hit, float.MaxValue,_gunStats.ContactLayer))
        {
            var enemyLife = hit.collider.gameObject.GetComponent<LifeController>();
            if(enemyLife != null)
                enemyLife.TakeDamage(_bulletStats.Damage);
            TrailRenderer trail = Instantiate(_gunStats.BulletTrail, startPos, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, hit));

        }
        currentTime = 0;
    }
    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit ray)
    {
        float time = 0;
        Vector3 startPos = trail.transform.position;
        while(time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPos, ray.point, time);
            time += Time.deltaTime / trail.time;
            yield return null;
        }
        trail.transform.position = ray.point;
        Destroy(trail.gameObject, trail.time);
    }
}
