using UnityEngine;

public class HunterChase : IState
{
    private Hunter _hunter;
    private FSM _manager;
    private float _chaseSpeedMultiplier = 1.3f;
    private float _energyLoss = 10f;


    public void OnAwake()
    {
        Debug.Log("Chasing");
        _hunter.speed *= _chaseSpeedMultiplier;
    }

    public void OnExecute()
    {
        if (_hunter.energy <= 0)
        {
            _manager.SetState<HunterIdle>();
            return;
        }

        _hunter.target.RemoveAll(item => item == null);

        if (_hunter.target.Count == 0)
        {
            _manager.SetState<HunterPatrol>();
            return;
        }

        Entity closestTarget = null;
        float closestDistance = float.MaxValue;

        foreach (Entity target in _hunter.target)
        {
            if (target == null) continue;

            float distance = Vector3.Distance(target.Position, _hunter.transform.position);



            if (distance < _hunter._visionRadius * 1.2f && distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = target;
            }
        }

        if (closestTarget != null)
        {
            Vector3 targetPosition = closestTarget.Position;
            Vector3 targetVelocity = closestTarget.Velocity;

            // Pursuit
            _hunter.SetVelocity(SteeringBehaviours.Pursuit(_hunter.transform.position, _hunter.speed, _hunter._directionalVelocity, targetPosition, targetVelocity, _hunter._steeringForce, _hunter.maxFutureTime));

            _hunter.transform.position += _hunter._directionalVelocity * Time.deltaTime;

            _hunter.energy -= _energyLoss * Time.deltaTime; // Loses energy while chasing
        }
        else
        {
            _manager.SetState<HunterPatrol>();
            return;
        }

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

