using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Evade es un steering behavior el cual nos permite calcular la direccion a un objetivo en una posicion futura,
/// basado en como se esta moviendo actualmente. Esto nos permitira predecir donde va a estar y asi poder evitar
/// esa direccion e intentar alejarnos de ella. Es el contrario de Pursuit.
/// </summary>
public class EvadeBoid : SteeringBehavior
{
    [SerializeField] private float _predictionTime = 0.5f;

    public override Vector3 CalculateDirection(List<Entity> targets)
    {
        if (!IsActive || targets.Count == 0) return Vector3.zero;
            return SteeringBoid.Evade(_owner, targets[0], _maxForce, _predictionTime) * Effectiveness;
    }

}
