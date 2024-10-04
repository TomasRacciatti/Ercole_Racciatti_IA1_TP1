using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodGetState : IState
{
    private Entity _food;
    private FSM _manager;
    private Boid _boid;
    private Flocking _flocking;
    private ArriveFinal _arriveFinal;
    private ObstacleAvoidanceBoid _obstacleAvoidance;
    private AreaManager _areaManager;
    private EvadeFinal _evade;
    private Entity _hunter;
    public void OnAwake()
    {
        _food = GameObject.FindGameObjectWithTag("food").GetComponent<Entity>();
        
        _boid = _boid.GetComponent<Boid>();
        _flocking = _boid.GetComponent<Flocking>();
        _arriveFinal = _boid.GetComponent<ArriveFinal>();
        _obstacleAvoidance = _boid.GetComponent<ObstacleAvoidanceBoid>();
        _evade = _boid.GetComponent<EvadeFinal>();
        
        _areaManager = Object.FindObjectOfType<AreaManager>();
        _hunter = GameObject.FindGameObjectWithTag("Hunter").GetComponent<Entity>();
    }

    public void OnExecute()
    {
        Debug.Log("Buscando comida");
        if (_areaManager._foodColision != null)
        {
            if (Vector3.Distance(_food.Position, _boid.transform.position) <= 20)
            {
                _arriveFinal._food = _food;
                _boid.GetComponent<SteeringBehavior>().IsActive = true;
                _flocking.GetComponent<SteeringBehavior>().IsActive = false;
                _arriveFinal.GetComponent<SteeringBehavior>().IsActive = true;
                _obstacleAvoidance.GetComponent<SteeringBehavior>().IsActive = true;
                _evade.GetComponent<SteeringBehavior>().IsActive = false;
            }
        }
        else if (Vector3.Distance(_hunter.Position, _boid.transform.position) > 20)
        {
            _manager.SetState<EvadeState>();
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
