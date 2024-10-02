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
        float distanceToTarget = Vector3.Distance(myPosition, targetPosition);
        float timeFactor = Mathf.Clamp01(distanceToTarget / maxTime);
        float futureTime = maxTime * timeFactor;

        Vector3 _futurePosition = targetPosition + targetVelocity * futureTime;

        return -Seek(myPosition, mySpeed, myVelocity, _futurePosition, force);
    }


    public static Vector3 Evade2(Vector3 myPosition, float mySpeed, Vector3 myVelocity, Vector3 targetPosition, Vector3 targetVelocity, float force, float maxTime)
    {
        return -Pursuit(myPosition, mySpeed, myVelocity, targetPosition, targetVelocity, force, maxTime);
    }
}
