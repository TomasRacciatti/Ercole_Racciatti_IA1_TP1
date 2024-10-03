using UnityEngine;

public class EvadeTest : Boid
{
    public Hunter target;


    private void Start()
    {
        target = FindAnyObjectByType<Hunter>();
    }

    private void Update()
    {
        if (target != null && Vector3.Distance(target.transform.position, transform.position) < _visionRadius)
        {
            SetVelocity(SteeringBehaviours.Evade(transform.position, speed, _directionalVelocity, target.Position, target.Velocity, _steeringForce, maxFutureTime));

            transform.position += _directionalVelocity * Time.deltaTime;
        }    
    }

}
