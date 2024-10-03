using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    public Vector3 Velocity { get => _directionalVelocity; }
    public Vector3 Position { get => transform.position; }
    
    [Header("Agent")]
    public Vector3 _directionalVelocity;
    public float speed;
    public float rotationSpeed;
    public float _visionRadius;
    [Range(0f, 1f)] public float _steeringForce;
    public float maxFutureTime = 2f;
    public LayerMask obstacleMask;



    public void SetVelocity(Vector3 force)
    {
        _directionalVelocity = Vector3.ClampMagnitude(_directionalVelocity + force, speed);
    }
}
