using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : Hunter
{
    private Vector3 _direction;
    [SerializeField] private int _targetPoint;
    private bool _isMovingForward = true;

    [SerializeField] private float _energyLoss;



    void Start()
    {
        _targetPoint = 0;
    }


    void Update()
    {
        energy -= _energyLoss * Time.deltaTime; // Loses energy while patrolling

        _direction = patrolPoints[_targetPoint].position - transform.position;

        if (_direction.magnitude < 0.1f)
        {
            ChangeWaypoint();
        }

        _direction.Normalize();
        transform.position += _direction * _speed * Time.deltaTime;

        if (_direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }


    void ChangeWaypoint()
    {
        if (_isMovingForward)
        {
            _targetPoint++;
            if (_targetPoint >= patrolPoints.Length)
            {
                _targetPoint = patrolPoints.Length - 1;
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
