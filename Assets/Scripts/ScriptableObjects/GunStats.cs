using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunStats", menuName = "Stats/GunStats", order = 3)]
public class GunStats : ScriptableObject
{
    public LayerMask ContactLayer => _contactLayer;
    [SerializeField] private LayerMask _contactLayer;

    public float ShootCooldown => _shootCooldown;
    [SerializeField] private float _shootCooldown;

    public TrailRenderer BulletTrail => _bulletTrail;
    [SerializeField] private TrailRenderer _bulletTrail;

    public ParticleSystem BulletExplodeParticles => _bulletExplodeParticles;
    [SerializeField] private ParticleSystem _bulletExplodeParticles;
}
