using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArriveFinal : SteeringBehavior
{
    public Entity _food;
    [SerializeField] private float maxSteeringForce;
    [SerializeField] private float arrivalRadius = 0.2f;

    public override Vector3 CalculateDirection(List<Entity> targets)
    {
        if (!IsActive) return Vector3.zero;
        if (_food == null) return Vector3.zero;
        
        return SteeringBoid.Arrive(_owner, _food, maxSteeringForce, arrivalRadius) * Effectiveness;
    }
}
