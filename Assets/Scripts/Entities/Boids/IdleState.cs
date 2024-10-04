using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor;
using UnityEngine;

public class IdleState : IState
{
    private FSM _manager;
    private Boid _boid;
    private Flocking _flocking;
    private ArriveFinal _arriveFinal;
    private ObstacleAvoidanceBoid _obstacleAvoidance;
    private EvadeFinal _evadeFinal;
    
    private Entity _hunter;
    
    private AreaManager _areaManager;
    public void OnAwake()
    {
        _boid = _boid.GetComponent<Boid>();
        _flocking = _boid.GetComponent<Flocking>();
        _arriveFinal = _boid.GetComponent<ArriveFinal>();
        _obstacleAvoidance = _boid.GetComponent<ObstacleAvoidanceBoid>();
        _evadeFinal = _boid.GetComponent<EvadeFinal>();
        
        _boid.GetComponent<SteeringBehavior>().IsActive = true;
        _flocking.GetComponent<SteeringBehavior>().IsActive = true;
        _arriveFinal.GetComponent<SteeringBehavior>().IsActive = false;
        _obstacleAvoidance.GetComponent<SteeringBehavior>().IsActive = true;
        _evadeFinal.GetComponent<SteeringBehavior>().IsActive = false;
        
        _areaManager = Object.FindObjectOfType<AreaManager>();
        _hunter = GameObject.FindGameObjectWithTag("hunter").GetComponent<Entity>();
    }

    public void OnExecute()
    {
        Debug.Log("Idle");
        
        _boid.GetComponent<SteeringBehavior>().IsActive = true;
        _flocking.GetComponent<SteeringBehavior>().IsActive = true;
        _arriveFinal.GetComponent<SteeringBehavior>().IsActive = false;
        _obstacleAvoidance.GetComponent<SteeringBehavior>().IsActive = true;
        _evadeFinal.GetComponent<SteeringBehavior>().IsActive = false;
        
        if (_areaManager._foodColision != null)
        {
            _manager.SetState<FoodGetState>();
            return;
        }

        if (Vector3.Distance(_hunter.Position, _boid.transform.position) <= 10)
        {
            _manager.SetState<EvadeState>();
        }
    }

    public void OnSleep()
    {
        return;
    }

    public void SetFSM(FSM manager)
    {
        _manager = manager;
    }

    public void SetAgent(Agent agent)
    {
        _boid = (Boid)agent;
    }
    

}
