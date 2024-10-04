using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeFinal : SteeringBehavior
{
    [SerializeField] private Hunter _hunter;
    [SerializeField] private float maxSteeringForce;
    [SerializeField] private float predictionTime;


    void Start()
    {
        _hunter = GameObject.Find("Hunter").GetComponent<Hunter>();
    }
    public override Vector3 CalculateDirection(List<Entity> targets)
    {
        if (!IsActive) return Vector3.zero;
        
        return SteeringBoid.Evade(_owner, _hunter, maxSteeringForce, predictionTime) * Effectiveness;
    }
}
