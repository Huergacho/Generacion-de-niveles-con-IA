using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IThief
{
    Transform transform { get; }
    ActorStats ActorStats { get; }
    IAStats IAStats { get; }
    PlayerModel Target { get; }
    Dictionary<SteeringType, ISteering> Behaviours { get; }
    ObstacleAvoidance Avoidance { get; }
    bool HasTakenDamage { get; }
    void Move(Vector3 dir, float desiredSpeed);
    void LookDir(Vector3 dir);

    //The main reason I did another interface:
    CollectableItem ItemStolen { get; }
    CollectableItem NextTarget { get; }
    void StealItem(CollectableItem item);
    void DropStolenItem();
    bool IsThereAnItemToSteal();
    void ReturnHomeDestination();
    void GetATarget();
}
