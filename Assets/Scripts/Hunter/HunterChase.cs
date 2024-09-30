using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterChase : IState
{
    private Hunter _hunter;
    private FSM _manager;

    [SerializeField] private float _energyLoss = 4f;


    public void OnAwake()
    {
        throw new System.NotImplementedException();
    }

    public void OnExecute()
    {
        if (_hunter.energy <= 20)
        {
            _manager.SetState<HunterIdle>();
            return;
        }

        if (_hunter.target == null || Vector3.Distance(_hunter.target.transform.position, _hunter.transform.position) > _hunter._visionRadius * 1.2f)
        {
            _manager.SetState<HunterPatrol>();
            return;
        }



        _hunter.energy -= _energyLoss * Time.deltaTime; // Loses energy while chasing
    }

    public void OnSleep()
    {
        throw new System.NotImplementedException();
    }

    public void SetAgent(Agent agent)
    {
        _hunter = (Hunter)agent;
    }

    public void SetFSM(FSM manager)
    {
        _manager = manager;
    }
}
