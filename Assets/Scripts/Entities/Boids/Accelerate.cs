using System.Collections.Generic;
using UnityEngine;

public class Accelerate : SteeringBehavior
{
    public override Vector3 CalculateDirection(List<Entity> targets)
    {
        if(!IsActive) return Vector3.zero;
        
        return _owner.Forward * (_maxForce * _effectiveness);
    }
}