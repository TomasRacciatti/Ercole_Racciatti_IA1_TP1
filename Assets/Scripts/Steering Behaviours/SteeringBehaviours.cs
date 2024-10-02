using UnityEngine;

public static class SteeringBehaviours
{
    public static Vector3 Seek(Vector3 hunterPosition, float hunterSpeed, Vector3 hunterVelocity, Vector3 targetPosition, float force)
    {
        // Block vertical movement
        hunterPosition.y = 0;
        targetPosition.y = 0;
        hunterVelocity.y = 0;


        Vector3 desiredDir = targetPosition - hunterPosition;
        desiredDir.Normalize();
        desiredDir *= hunterSpeed;

        Vector3 steeringDir = desiredDir - hunterVelocity;
        steeringDir.Normalize();
        steeringDir *= force;

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

    public static Vector3 Evade (Vector3 myPosition, float mySpeed, Vector3 myVelocity, Vector3 targetPosition, Vector3 targetVelocity, float force, float maxTime)
    {
        Vector3 _futurePosition = targetPosition + targetVelocity * maxTime;

        return -Seek(myPosition, mySpeed, myVelocity, _futurePosition, force);
    }
}
