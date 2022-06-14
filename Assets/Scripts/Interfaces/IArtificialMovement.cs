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
    bool HasTakenDamage { get; }
    GameObject[] PatrolRoute { get; }
    void Move(Vector2 dir, float desiredSpeed);
    void SmoothRotation(Vector3 dest);
    void TakeHit(bool value =  true);
    void Shoot();
    bool IsInShootingRange();
    bool IsTargetInSight();
    void LookDir(Vector3 dir);
}
