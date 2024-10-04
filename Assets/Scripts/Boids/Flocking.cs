using System.Collections.Generic;
using UnityEngine;

public class Flocking : SteeringBehavior
{
    private static GlobalFlockingSettings _flockingSettings;
    
    private Vector3 _debugLocation;
    public bool shouldDebug = false;

    private void Awake()
    {
        if (_flockingSettings == null)
            _flockingSettings = Resources.Load<GlobalFlockingSettings>("GlobalFlockingSettings");
        
    }

    public override Vector3 CalculateDirection(List<Entity> targets)
    {
        if (!IsActive || targets.Count == 0) return Vector3.zero;

        var separation = SteeringBoid.Separation(_owner, targets, _flockingSettings.SeparationRadius, _maxForce * _flockingSettings.SeparationWeight);
        var alignment = SteeringBoid.Alignment(_owner, targets, _maxForce * _flockingSettings.AlignmentWeight);
        var cohesion = SteeringBoid.Cohesion(_owner, targets, _maxForce * _flockingSettings.CohesionWeight, out _debugLocation);

        if (shouldDebug)
        {
            if(separation != Vector3.zero) Debug.DrawRay(transform.position, separation.normalized, Color.yellow);
            if(alignment != Vector3.zero) Debug.DrawRay(transform.position, alignment.normalized, Color.red);
            if(cohesion != Vector3.zero) Debug.DrawRay(transform.position, cohesion.normalized, Color.cyan);
        }
        
        return (separation + alignment + cohesion) * Effectiveness;
    }
    
    public static List<Entity> DetectBoids(Entity mainBoid)
    {
        var boids = new List<Entity>();
        if (!_flockingSettings) _flockingSettings = Resources.Load<GlobalFlockingSettings>("GlobalFlockingSettings");

        var colliders = Physics.OverlapSphere(mainBoid.Position, _flockingSettings.ViewRadius, _flockingSettings.BoidMask, QueryTriggerInteraction.Collide);
        if (colliders.Length == 0 || (colliders.Length == 1 && colliders[0].gameObject == mainBoid.gameObject)) 
            return boids;
        
        foreach (var col in colliders)
        {
            var dir = col.transform.position - mainBoid.Position;
            if (Vector3.Angle(dir, mainBoid.Forward) < _flockingSettings.ViewAngle)
            {
                var boid = col.gameObject.GetComponentInParent<Boid>();
                if (boid)
                {
                    boids.Add(boid);
                }
            }
        }
        return boids;
    }
    
    private void OnDrawGizmosSelected()
    {
        if (IsActive && _flockingSettings != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(_debugLocation, 0.25f);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _flockingSettings.SeparationRadius);
            
            Gizmos.color = new Color(1f, 0.5f, 0f);
            Gizmos.DrawWireSphere(transform.position, _flockingSettings.ViewRadius);

            var right = Quaternion.Euler(0, _flockingSettings.ViewAngle, 0) * transform.forward;
            var left = Quaternion.Euler(0, -_flockingSettings.ViewAngle, 0) * transform.forward;
            Gizmos.DrawRay(transform.position, right.normalized * _flockingSettings.ViewRadius);
            Gizmos.DrawRay(transform.position, left.normalized * _flockingSettings.ViewRadius);
        }
    }
}