using UnityEngine;

public class TestSteeringBehaviours : Agent
{
    public Agent target;


    private void Start()
    {
        //SetVelocity(new Vector3(0, 0, speed));   Testing
    }

    private void Update()
    {
        SetVelocity(PursuitTest(target.Position, _steeringForce, maxFutureTime));

        transform.position += _directionalVelocity * Time.deltaTime;
    }

    private Vector3 SeekTest(Vector3 targetPosition, float force)
    {
        Vector3 desiredDir = targetPosition - transform.position;
        desiredDir.Normalize();
        desiredDir *= speed;

        Vector3 steeringDir = desiredDir - _directionalVelocity;
        steeringDir.Normalize();
        steeringDir *= force;



        // Visualizacion para chequear
        Debug.DrawRay(transform.position, _directionalVelocity.normalized, Color.green);
        Debug.DrawRay(transform.position, desiredDir.normalized, Color.red);
        Debug.DrawRay(transform.position + _directionalVelocity.normalized, steeringDir, Color.blue);

        return steeringDir;
    }

    private Vector3 PursuitTest(Vector3 targetPosition, float force, float maxTime)
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        float timeFactor = Mathf.Clamp01(distanceToTarget / maxTime);
        float futureTime = maxTime * timeFactor;

        Vector3 _futurePosition = targetPosition + target.Velocity * futureTime;

        return SeekTest(_futurePosition, force);
    }

    private void OnDrawGizmos()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.Position);
        float timeFactor = Mathf.Clamp01(distanceToTarget / maxFutureTime);
        float futureTime = maxFutureTime * timeFactor;

        Vector3 pursuitPosition = target.Position + target.Velocity * futureTime;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(pursuitPosition, 0.5f);
    }
}
