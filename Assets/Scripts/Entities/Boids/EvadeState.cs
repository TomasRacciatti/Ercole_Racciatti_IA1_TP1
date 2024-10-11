using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeState : IState
{
    private Entity _food;
    private FSM _manager;
    private Boid _boid;
    private Flocking _flocking;
    private ArriveFinal _arriveFinal;
    private ObstacleAvoidanceBoid _obstacleAvoidance;
    private EvadeFinal _evade;
    private AreaManager _areaManager;
    private Entity _hunter;

    public void OnAwake()
    {
        _boid = _boid.GetComponent<Boid>();
        _flocking = _boid.GetComponent<Flocking>();
        _arriveFinal = _boid.GetComponent<ArriveFinal>();
        _obstacleAvoidance = _boid.GetComponent<ObstacleAvoidanceBoid>();
        _evade = _boid.GetComponent<EvadeFinal>();

        _boid.GetComponent<SteeringBehavior>().IsActive = true;
        _flocking.GetComponent<SteeringBehavior>().IsActive = true;
        _arriveFinal.GetComponent<SteeringBehavior>().IsActive = false;
        _obstacleAvoidance.GetComponent<SteeringBehavior>().IsActive = false;
        _evade.GetComponent<SteeringBehavior>().IsActive = false;

        _areaManager = Object.FindObjectOfType<AreaManager>();
        _hunter = GameObject.FindGameObjectWithTag("hunter").GetComponent<Entity>();
    }

    public void OnExecute()
    {
        Debug.Log("Evadiendo");
        if (Vector3.Distance(_hunter.Position, _boid.transform.position) < 10)
        {
            _evade.GetComponent<SteeringBehavior>().IsActive = true;
            _boid.GetComponent<SteeringBehavior>().IsActive = true;
            _flocking.GetComponent<SteeringBehavior>().IsActive = false;
            _arriveFinal.GetComponent<SteeringBehavior>().IsActive = false;
            _obstacleAvoidance.GetComponent<SteeringBehavior>().IsActive = true;
        }

        else if (Vector3.Distance(_hunter.Position, _boid.transform.position) > 10 &&
                 _areaManager._foodColision != null)
        {
            _manager.SetState<FoodGetState>();
            return;
        }
        else
        {
            _manager.SetState<IdleState>();
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