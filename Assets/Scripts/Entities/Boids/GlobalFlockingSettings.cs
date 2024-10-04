using UnityEngine;

/// <summary>
/// Contiene las settings que vamos a usar para el Flocking, esta clase hereda de ScriptableObject por lo que es un
/// objeto que vive en memoria (es decir, en nuestro harddrive) y para usarlo tendriamos que cargarlo en memoria ram.
/// Para lo que queremos saber por ahora vamos a cargarlo desde la clase Flocking y ahi se levanta desde el disco en
/// una variable estatica, y nos podemos olvidar. Ahora bien, estos objetos son modificables en runtime, pero preferiria
/// mos usarlos como una fuente de datos (seteo lo datos desde el editor y los leo desde el codigo)
/// </summary>
[CreateAssetMenu(menuName = "AI1/FlockingData", fileName = "GlobalFlockingSettings")]
public class GlobalFlockingSettings : ScriptableObject
{
    public LayerMask BoidMask => _boidMask;
    public float ViewRadius => _viewRadius;
    public float ViewAngle => _viewAngle;
    public float SeparationRadius => _separationRadius;
    
    public float SeparationWeight => _separationWeight;
    public float AlignmentWeight => _alignmentWeight;
    public float CohesionWeight => _cohesionWeight;
    
    [Header("Settings")]
    [SerializeField] private LayerMask _boidMask;
    [SerializeField] private float _viewRadius = 3;
    [SerializeField] private float _viewAngle = 120;
    [SerializeField] private float _separationRadius = 1;
    
    [Header("Steering")]
    [Range(0f, 3f), SerializeField] private float _separationWeight = 2.41f;
    [Range(0f, 3f), SerializeField] private float _alignmentWeight = 1.18f;
    [Range(0f, 3f), SerializeField] private float _cohesionWeight = 1f;
}