using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActorStats", menuName = "Stats/ActorStats", order = 0)]
public class ActorStats : ScriptableObject
{
    [SerializeField] private float maxLife = 100f;
    public float MaxLife => maxLife;

    [SerializeField] private float walkSpeed = 1f;
    public float WalkSpeed => walkSpeed;

    [SerializeField] private float runSpeed = 2f;
    public float RunSpeed => runSpeed;

    [SerializeField] private float rotationSpeed = 0.1f;
    public float RotSpeed => rotationSpeed;

    [SerializeField] private float shootCooldown = 0.5f;
    public float ShootCooldown => shootCooldown;

    [SerializeField] private float _angleVision = 90f;
    public float AngleVision => _angleVision;

    [SerializeField] private float _rangeVision = 5f;
    public float RangeVision => _rangeVision;
}
