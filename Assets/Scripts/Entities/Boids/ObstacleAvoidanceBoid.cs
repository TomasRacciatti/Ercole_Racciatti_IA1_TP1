using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidanceBoid : SteeringBehavior
{
    [SerializeField] private float _castRadius = 0.2f;
    [SerializeField] private float _aheadDistance = 3f;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private float _angle = 45f;

    public override Vector3 CalculateDirection(List<Entity> targets)
    {
        if (!IsActive) return Vector3.zero;
        
        return SteeringBoid.ObstacleAvoidance(_owner, _maxForce, _castRadius, _aheadDistance, _obstacleMask, _angle) * Effectiveness;
    }
}