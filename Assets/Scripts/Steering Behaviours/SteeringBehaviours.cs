using UnityEngine;

public static class SteeringBehaviours
{
    public static Vector3 Seek(Vector3 hunterPosition, float hunterSpeed, Vector3 hunterVelocity, Vector3 targetPosition, float force)
    {
        Vector3 desiredDir = targetPosition - hunterPosition;
        desiredDir.Normalize();
        desiredDir *= hunterSpeed;

        Vector3 steeringDir = desiredDir - hunterVelocity;
        steeringDir.Normalize();
        steeringDir *= force;

        steeringDir.y = 0; // Block vertical movement

        return steeringDir;
    }

    public static Vector3 Pursuit(Vector3 hunterPosition, float hunterSpeed, Vector3 hunterVelocity, Vector3 targetPosition, Vector3 targetVelocity, float force, float maxTime)
    {
        float distanceToTarget = Vector3.Distance(hunterPosition, targetPosition);
        float timeFactor = Mathf.Clamp01(distanceToTarget / maxTime);
        float futureTime = maxTime * timeFactor;

        Vector3 _futurePosition = targetPosition + targetVelocity * futureTime;

        return Seek(hunterPosition, hunterSpeed, hunterVelocity, _futurePosition, force);
    }

    public static Vector3 Evade(Vector3 myPosition, float mySpeed, Vector3 myVelocity, Vector3 targetPosition, Vector3 targetVelocity, float force, float maxTime)
    {
        return -Pursuit(myPosition, mySpeed, myVelocity, targetPosition, targetVelocity, force, maxTime);
    }

    public static Vector3 ObstacleAvoidance(Agent agent, float force, float castRadius, float aheadDistance, LayerMask obstacleMask, float rotAngle)
    {

        if (Physics.SphereCast(agent.Position, castRadius, agent.Velocity, out RaycastHit hitInfo, aheadDistance, obstacleMask))
        {
            Transform obstacle = hitInfo.transform;
            Vector3 dirToObject = obstacle.position - agent.Position;
            float angleInBetween = Vector3.SignedAngle(agent.Velocity, dirToObject, Vector3.up);

            float rotationValue = angleInBetween >= 0 ? -rotAngle : rotAngle;  // if angle >= 0 --> -rotAngle. Else --> rotAngle
            Vector3 desiredDir = Quaternion.Euler(0, rotationValue, 0) * agent.Velocity;
            desiredDir.Normalize();
            desiredDir *= agent.speed;

            Vector3 steering = desiredDir - agent.Velocity;
            steering *= force;

            return steering;
        }

        return Vector3.zero;
    }
}
