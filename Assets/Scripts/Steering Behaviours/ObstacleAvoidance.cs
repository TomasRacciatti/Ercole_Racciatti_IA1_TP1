using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance : MonoBehaviour
{

    [SerializeField] private float _castRadius = 0.2f;
    [SerializeField] private float _aheadDistance = 5f;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private float _angle = 45f;
    [SerializeField] private Agent _agent;


    private void Start()
    {
        _agent = GetComponent<Agent>();
    }

    private void Update()
    {
        _agent.SetVelocity(SteeringBehaviours.ObstacleAvoidance(_agent, _agent._steeringForce, _castRadius, _aheadDistance, _obstacleMask, _angle));
    }

}
