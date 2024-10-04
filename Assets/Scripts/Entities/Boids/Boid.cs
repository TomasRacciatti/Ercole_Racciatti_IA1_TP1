using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boid : AgentBoid
{
    private readonly HashSet<Type> _steeringSettings = new HashSet<Type>();

    //Borrar
    /*
    public Hunter _hunter;
    [Range(0f, 1f)] public float _steeringForce;
    public float maxFutureTime = 0.5f;
    */

    protected override void Awake()
    {
        base.Awake();

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

        //Borrar esto despues
        //AddForce(SteeringBehaviours.Evade(transform.position, _maxSpeed, Velocity, _hunter.Position, _hunter.Velocity, _steeringForce, maxFutureTime));
    }
}