using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IArtificialMovement
{
    LifeController LifeController { get; }
    ActorStats ActorStats { get; }
    IAStats IAStats { get; }
    Dictionary<SteeringType, ISteering> Behaviours { get; }
    ObstacleAvoidance Avoidance { get; }
    Transform transform { get; }
    PlayerModel Target { get; }
    LineOfSight LineOfSight { get; }
    Action<bool> OnDetect { get; set; }
    bool HasTakenDamage { get; }
    GameObject[] PatrolRoute { get; }
    Vector3 Destination { get; }
    int RamdonizeTargetInPatrolRoute();
    void Move(Vector3 dir, float desiredSpeed);
    void TakeHit(bool value);
    void Shoot();
    bool IsInShootingRange();
    bool IsTargetInSight();
    void LookDir(Vector3 dir);
    bool FarFromDestination();
    bool IsEnemyFar();
}
