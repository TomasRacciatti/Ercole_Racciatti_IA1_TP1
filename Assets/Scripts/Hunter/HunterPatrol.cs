using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterPatrol : IState
{
    private Hunter _hunter;
    private FSM _manager;

    private Vector3 _direction;
    [SerializeField] private int _targetPoint = 0;
    private bool _isMovingForward = true;
    [SerializeField] private float _energyLoss = 1.5f;


    public void OnAwake()
    {
        return;
    }

    public void OnExecute()
    {

        if (_hunter.energy <= 0)
        {
            _manager.SetState<HunterIdle>();
            return;
        }

        if (_hunter.target != null && _hunter.energy >= 20 && Vector3.Distance(_hunter.target.transform.position, _hunter.transform.position) <= _hunter._visionRadius)
        {
            _manager.SetState<HunterChase>();
            return;
        }


        _hunter.energy -= _energyLoss * Time.deltaTime; // Loses energy while patrolling

        _direction = _hunter.patrolPoints[_targetPoint].position - _hunter.transform.position;

        if (_direction.magnitude < 0.1f)
        {
            ChangeWaypoint();
        }

        _direction.Normalize();
        _hunter.transform.position += _direction * _hunter._speed * Time.deltaTime;

        if (_direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_direction);
            _hunter.transform.rotation = Quaternion.Slerp(_hunter.transform.rotation, targetRotation, _hunter._rotationSpeed * Time.deltaTime);
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
    }
}
