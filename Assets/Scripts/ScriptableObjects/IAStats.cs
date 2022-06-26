using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IAStats", menuName = "Stats/IAStats", order = 2)]
public class IAStats : ScriptableObject
{
    public CollectableItem CollectablePrefab => _collectablePrefab;
    [SerializeField] private CollectableItem _collectablePrefab;

    public float ShootDistance => shootDistance;
    [SerializeField] private float shootDistance = 2f;

    public float TimeRoot => _timeRoot;
    [SerializeField] float _timeRoot = 1f;

    public bool CanReversePatrol => _canReversePatrol;
    [SerializeField] private bool _canReversePatrol = false;

    //STEERING
    public float MaxDistanceFromTarget => _maxDistanceFromTarget;
    [SerializeField] private float _maxDistanceFromTarget = 5f;

    public float SteeringTime => steeringTime;
    [SerializeField] private float steeringTime = 5f;

    public float AngleAvoidance => _angleAvoidance;
    [SerializeField] private float _angleAvoidance = 270f;
    public float RangeAvoidance => _rangeAvoidance;
    [SerializeField] private float _rangeAvoidance = 1f;

    public float SteeringWeight => _steeringWeight;
    [SerializeField] private float _steeringWeight = 1f;

    public float AvoidanceWeight => _avoidanceWeight;
    [SerializeField] private float _avoidanceWeight = 1f;

    public float PredictionTime => predictionTime;
    [SerializeField] private float predictionTime = 1f;

    public float NearTargetRange => _rangeHome;
    [SerializeField] private float _rangeHome = 0.5f;

    public float RandomAngleWandering => _randomAngleWandering;
    [SerializeField] private float _randomAngleWandering = 35f;
}
