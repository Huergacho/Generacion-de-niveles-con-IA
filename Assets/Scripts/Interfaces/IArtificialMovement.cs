using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IArtificialMovement
{
    ActorStats ActorStats { get; }
    IAStats IAStats { get; }
    Dictionary<SteeringType, ISteering> Behaviours { get; }
    ObstacleAvoidance Avoidance { get; }
    Transform transform { get; }
    PlayerModel Target { get; }
    LineOfSight LineOfSight { get; }
    public void Move(Vector2 dir, float desiredSpeed);
    public void SmoothRotation(Vector3 dest);
}
