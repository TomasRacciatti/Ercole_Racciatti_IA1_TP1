using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : Agent
{
    [Header("Hunter")]
    public float energy;
    public float _visionRadius;
    public Transform[] patrolPoints;

    public Agent target; // CORREGIR CUANDO ESTEN LOS BOIDS

    public FSM stateMachine;


    private void Awake()
    {
        stateMachine = new();

        
        stateMachine.AddNewState<HunterIdle>().SetAgent(this);
        stateMachine.AddNewState<HunterPatrol>().SetAgent(this);
        stateMachine.AddNewState<HunterChase>().SetAgent(this);

        stateMachine.SetInitialState<HunterPatrol>();
        
    }


    private void Update()
    {
        stateMachine.OnUpdate();
    }
}
