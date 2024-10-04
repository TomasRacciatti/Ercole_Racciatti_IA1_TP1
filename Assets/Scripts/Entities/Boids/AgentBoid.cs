using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class AgentBoid : Entity
{

    public override Vector3 Velocity => _velocity;
    
    public float MaxSpeed => _maxSpeed;
    
    [Header("Agent-boid")]
    [SerializeField] protected float _maxSpeed = 5f;
    [SerializeField] protected Vector3 _velocity = Vector3.zero;

    protected readonly Dictionary<Type, SteeringBehavior> _steeringBehaviors = new();

    protected virtual void Awake()
    {
        var behaviors = GetComponents<SteeringBehavior>();

        foreach (var behavior in behaviors)
        {
            behavior.Initialize(this);
            _steeringBehaviors.Add(behavior.GetType(), behavior);
        }

        _steeringBehaviors.TrimExcess();
    }

    public Vector3 GetSteeringDirection(List<Entity> entities)
    {
        var steering = Vector3.zero;

        foreach (var behavior in _steeringBehaviors)
        {
            steering += behavior.Value.CalculateDirection(entities);
        }

        return Vector3.ClampMagnitude(steering, MaxSpeed);
    }

    public void SetBehaviorEffectiveness(Type behaviorType, float effectiveness, bool ignoreInactive = true)
    {
        if (_steeringBehaviors.TryGetValue(behaviorType, out var behavior))
        {
            if (!behavior.IsActive && ignoreInactive) return;
            behavior.Effectiveness = effectiveness;
        }
    }
    public void UpdateBehaviorsActiveState(HashSet<Type> behaviorToActivate)
    {
        foreach (var behavior in _steeringBehaviors)
        {
            behavior.Value.IsActive = behaviorToActivate.Contains(behavior.Key);
        }
    }

    public void AddForce(Vector3 force)
    {
        _velocity = AddForce(force, _velocity);
    }

    public Vector3 AddForce(Vector3 force, Vector3 toVector)
    {
        return Vector3.ClampMagnitude(toVector + force, _maxSpeed);
    }
    protected void ApplyVelocity(bool changeRotation = false)
    {
        if(changeRotation) transform.forward = _velocity;
        transform.position += _velocity * Time.deltaTime;

        UpdateAreaLimits();
        UpdateAreaColliders();
    }
}
