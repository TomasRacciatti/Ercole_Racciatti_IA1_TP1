using UnityEngine;

public class HunterPatrol : IState
{
    private Hunter _hunter;
    private FSM _manager;

    private int _targetPoint = 0;
    private bool _isMovingForward = true;
    private float _energyLoss = 1.5f;


    public void OnAwake()
    {
        _hunter.patroling.gameObject.SetActive(true);
        return;
    }

    public void OnExecute()
    {

        if (_hunter.energy <= 0)
        {
            _manager.SetState<HunterIdle>();
            return;
        }

        if (_hunter.target != null && _hunter.energy >= 0)
        {
            foreach (Entity target in _hunter.target)
            {
                Vector3 targetPosition = target.Position;

                if (Vector3.Distance(targetPosition, _hunter.transform.position) <= _hunter._visionRadius)
                {
                    _manager.SetState<HunterChase>();
                    return;
                }
            }
        }

        _hunter.energy -= _energyLoss * Time.deltaTime; // Loses energy while patrolling

        Vector3 desiredVelocity = SteeringBehaviours.Seek(_hunter.transform.position, _hunter.speed, _hunter._directionalVelocity, _hunter.patrolPoints[_targetPoint].position, _hunter._steeringForce);
        _hunter.SetVelocity(desiredVelocity);

        _hunter.transform.position += _hunter._directionalVelocity * Time.deltaTime;

        if (Vector3.Distance(_hunter.transform.position, _hunter.patrolPoints[_targetPoint].position) < 0.5f)
        {
            ChangeWaypoint();
        }


    }

    public void OnSleep()
    {
        _hunter.patroling.gameObject.SetActive(false);
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

    void ChangeWaypoint()
    {
        if (_isMovingForward)
        {
            _targetPoint++;
            if (_targetPoint >= _hunter.patrolPoints.Length)
            {
                _targetPoint = _hunter.patrolPoints.Length - 1;
                _isMovingForward = false;
            }
        }
        else
        {
            _targetPoint--;
            if (_targetPoint <= 0)
            {
                _targetPoint = 0;
                _isMovingForward = true;
            }
        }

        //Debug.Log("current waypoint: " + _targetPoint);
    }
}
