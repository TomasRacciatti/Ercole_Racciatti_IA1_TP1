using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Boid : AgentBoid
{
    private readonly HashSet<Type> _steeringSettings = new HashSet<Type>();
    private EvadeArrive _evade;
    protected override void Awake()
    {
        base.Awake();
        _evade = GetComponent<EvadeArrive>();
        _steeringSettings.Add(typeof(Flocking));
        _steeringSettings.Add(typeof(ObstacleAvoidanceBoid));

        UpdateBehaviorsActiveState(_steeringSettings);
    }

    protected override void Start()
    {
        base.Start();
        AddForce(new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * MaxSpeed);
    }

    private void Update()
    {
        var boids = Flocking.DetectBoids(this);
        
        SetBehaviorEffectiveness(typeof(Accelerate), boids.Count == 0 ? 1f : 0f, false);
        var dir = GetSteeringDirection(boids);
        AddForce(dir);
        ApplyVelocity(true);
    }
}