using System;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Manager del area de simulacion.
/// </summary>
public class AreaManager : MonoBehaviour
{
    // Esta es una setup de Singleton con protecciones. Solo un elemento de esta clase puede setearse.
    public static AreaManager Instance { get; private set; }
    public static Action foodOn;
    public FoodColision _foodColision;
    public Vector2 Dimensions => _dimensions;
    public Vector2 HalfDimensions => _dimensions / 2f;
    
    [SerializeField] private Vector2 _dimensions;
    [SerializeField] private Entity _foodPrefab;
    [SerializeField] private int _foodAmount;
    [SerializeField] private Boid _boidPrefab;
    [SerializeField] private int _boidSpawnAmount;
    
    private void Awake()
    {
        // Si no hay una Instance, yo soy el instance.
        if (Instance == null)
        {
            Instance = this;
        }
        // Si hay una Instance y no soy yo, me destruyo.
        else if (Instance != this)
        {
            Destroy(this); //Destruye el script, no el objeto.
        }
    }

    private void Start()
    {
        //Spawneamos los boids y unos obstaculos de manera aleatoria.
        Spawn(_boidSpawnAmount, _boidPrefab);
    }

    /// <summary>
    /// Spawneamos de manera aleatoria una cierta cantidad de objetos en escena.
    /// </summary>
    /// <param name="spawnAmount">Cantidad de objetos a spawnear</param>
    /// <param name="prefab">Prefab que vamos a usar para spawnear</param>
    private void Spawn<T>(int spawnAmount, T prefab) where T : MonoBehaviour
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            var halfDim = HalfDimensions;
            var instance = Instantiate(prefab);
            var posX = Random.Range(-halfDim.x,halfDim.x);
            var posY = Random.Range(-halfDim.y,halfDim.y);

            var randomPosition = new Vector3(posX, 0, posY);
            var randomRotation = Quaternion.Euler(new Vector3(0, Random.Range(0f, 360f), 0));

            instance.transform.position = randomPosition;
            instance.transform.rotation = randomRotation;
        }
    }

    public void ButtonFood()
    {
        if (_foodColision == null)
        {
            Spawn(_foodAmount, _foodPrefab);
        }
        else
        {
            Debug.Log("You can only spawn one food");
        }
    }
    
    private void OnDrawGizmos()
    {
        // Dibujamos los bordes el area.
        Gizmos.color = Color.yellow;
        
        var halfDim = HalfDimensions;
        var topLeft = new Vector3(-halfDim.x, 0, halfDim.y);
        var topRight = new Vector3(halfDim.x, 0, halfDim.y);
        var bottomLeft = new Vector3(-halfDim.x, 0, -halfDim.y);
        var bottomRight = new Vector3(halfDim.x, 0, -halfDim.y);
        
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topLeft, bottomLeft);
        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight);
    }
}