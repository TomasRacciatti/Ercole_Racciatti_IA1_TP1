using UnityEngine;

public class HunterIdle : IState
{
    private Hunter _hunter;
    private FSM _manager;

    [SerializeField] private float _energyRegain = 10;
    
    public void OnAwake()
    {
        return;
    }

    public void OnExecute()
    {
        _hunter.energy += _energyRegain * Time.deltaTime;

        if (_hunter.energy >= _hunter.maxEnergy)
        {
            _manager.SetState<HunterPatrol>();
            return;
        }

        if (_hunter.target != null && _hunter.energy >= _hunter.maxEnergy && Vector3.Distance(_hunter.target.transform.position, _hunter.transform.position) <= _hunter._visionRadius)
        {
            _manager.SetState<HunterChase>();
            return;
        }
    }

    public void OnSleep()
    {
        return;
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
