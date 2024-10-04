using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Boid : AgentBoid
{
    public FSM stateMachine;
    private readonly HashSet<Type> _steeringSettings = new HashSet<Type>();
    private Hunter _hunter;
    

    protected override void Awake()
    {
        base.Awake();

        _steeringSettings.Add(typeof(Flocking));
        _steeringSettings.Add(typeof(ObstacleAvoidanceBoid));
        _steeringSettings.Add(typeof(EvadeFinal));
        _steeringSettings.Add(typeof(ArriveFinal));

        UpdateBehaviorsActiveState(_steeringSettings);
        
        stateMachine = new();


        stateMachine.AddNewState<IdleState>().SetAgent(this);
        stateMachine.AddNewState<FoodGetState>().SetAgent(this);
        stateMachine.AddNewState<EvadeState>().SetAgent(this);

        stateMachine.SetInitialState<IdleState>();
    }

    protected override void Start()
    {
        base.Start();
        AddForce(new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * MaxSpeed);
    }

    private void Update()
    {
        stateMachine.OnUpdate();
        var boids = Flocking.DetectBoids(this);
        
        SetBehaviorEffectiveness(typeof(Accelerate), boids.Count == 0 ? 1f : 0f, false);
        var dir = GetSteeringDirection(boids);
        AddForce(dir);
        ApplyVelocity(true);
    }
}