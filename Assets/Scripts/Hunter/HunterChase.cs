using UnityEngine;

public class HunterChase : IState
{
    private Hunter _hunter;
    private FSM _manager;
    private float _chaseSpeedMultiplier = 1.3f;
    private float maxFutureTime = 10f;
    private float _energyLoss = 10f;


    public void OnAwake()
    {
        _hunter.speed *= _chaseSpeedMultiplier;
    }

    public void OnExecute()
    {
        if (_hunter.energy <= 0)
        {
            _manager.SetState<HunterIdle>();
            return;
        }

        if (_hunter.target == null || Vector3.Distance(_hunter.target.transform.position, _hunter.transform.position) > _hunter._visionRadius * 1.2f)
        {
            _manager.SetState<HunterPatrol>();
            return;
        }
        
        // Pursuit
        _hunter.SetVelocity(SteeringBehaviours.Pursuit(_hunter.transform.position, _hunter.speed, _hunter._directionalVelocity,_hunter.target.Position, _hunter.target.Velocity, _hunter._steeringForce, maxFutureTime));

        _hunter.transform.position += _hunter._directionalVelocity * Time.deltaTime;

        _hunter.energy -= _energyLoss * Time.deltaTime; // Loses energy while chasing
    }

    public void OnSleep()
    {
        _hunter.speed /= _chaseSpeedMultiplier;
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
