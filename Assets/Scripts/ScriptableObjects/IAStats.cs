using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IAStats", menuName = "Stats/IAStats", order = 2)]
public class IAStats : ScriptableObject
{
    public LayerMask TargetLayer => _targetLayer;
    [SerializeField] private LayerMask _targetLayer;

    public LayerMask ObstacleLayers => obstacleLayers;
    [SerializeField] private LayerMask obstacleLayers;

    public float DetectionDistance => _detectionDistance;
    [SerializeField] private float _detectionDistance;

    public float ShootCoooldown => shootCooldown;
    [SerializeField] private float shootCooldown = 1f;

    public float TimeRoot => _timeRoot;
    [SerializeField] float _timeRoot = 1f;

    public float AngleAvoidance => _angleAvoidance;
    [SerializeField] private float _angleAvoidance = 270f;
    public float RangeAvoidance => _rangeAvoidance;
    [SerializeField] private float _rangeAvoidance = 1f;

    public float SteeringWeight => _steeringWeight;
    [SerializeField] private float _steeringWeight = 1f;

    public float AvoidanceWeight => _avoidanceWeight;
    [SerializeField] private float _avoidanceWeight = 1f;

    public float TimePrediction => _timePrediction;
    [SerializeField] private float _timePrediction = 5f;

    public float MaxDistanceFromTarget => _maxDistanceFromTarget;
    [SerializeField] private float _maxDistanceFromTarget = 5f;

    public bool CanReversePatrol => _canReversePatrol;
    [SerializeField] private bool _canReversePatrol;

    public float ShootDistance => shootDistance;
    [SerializeField] private float shootDistance = 2f;

    public float PredictionTime => predictionTime;
    [SerializeField] private float predictionTime = 1f;

    public float AvoidanceMult => avoidanceMult;
    [SerializeField] private float avoidanceMult = 1f;

    public float BehaviourMult => behaviourMult;
    [SerializeField] private float behaviourMult = 1f;

    public float SteeringTime => steeringTime;
    [SerializeField] private float steeringTime = 5f;

    public Dictionary<SteeringType, int> SteeringSpeed => steeringSpeed;
    [SerializeField] private Dictionary<SteeringType, int> steeringSpeed;
    
}
