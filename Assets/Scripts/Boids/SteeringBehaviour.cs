using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase base para todos los steering behaviors que vamos a usar. Nos permite poder contener todos los tipos distintos
/// en una misma clase base, y asi poder iterarlos e ir calculando sus direcciones. 
/// </summary>
public abstract class SteeringBehavior : MonoBehaviour
{
    // Referencia al Agente sobre el cual aplicamos los steering behaviors.
    protected AgentBoid _owner;
    
    // Helpers con los que podemos saber y cambiar el estado de activacion y el valor de effectividad.
    public bool IsActive 
    {
        get => _isActive;
        set => _isActive = value;
    }
    public float Effectiveness
    {
        get => _effectiveness;
        // Con esto podemos cambiar la effectividad de un behavior por el codigo.
        set => _effectiveness = Mathf.Clamp01(value);
    }
    
    [SerializeField] protected bool _isActive = false;
    [SerializeField] protected float _maxForce = 0.08f;
    [SerializeField, Range(0f, 1f)] protected float _effectiveness = 1f;

    /// <summary>
    /// Inicializador, guardamos al "owner" como referencia.
    /// </summary>
    /// <param name="owner">gente sobre el cual aplicamos los steering behaviors.</param>
    public virtual void Initialize(AgentBoid owner)
    {
        _owner = owner;
    }
    
    /// <summary>
    /// Calcula la direccion deseada del steering behavior basado en sus paramtros internos y una lista de targets.
    /// </summary>
    /// <param name="targets">Targets que pueden tener en cuenta los steering behaviors.</param>
    /// <returns>Direccion deseada por la cual cambiar nuestro velocity.</returns>
    public abstract Vector3 CalculateDirection(List<Entity> targets);

}