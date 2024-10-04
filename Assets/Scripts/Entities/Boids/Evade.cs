using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Evade : AgentBoid
{
    [Tooltip("Maxima fuerza aplicable por el steering behavior. (porcentual 1 = 100%)")]
    [Range(0f, 1f)] public float maxSteeringForce;

    [Tooltip("Tiempo en segundos que vamos a usar para predecir la posicion futura en Pursuit y Evade.")]
    public float futureTime = 1f;
    
    [Tooltip("Radio desde el cual empezamos a frenar al usar Arrive.")]
    public float arriveRadius = 5f;
    
    [Tooltip("Agente al cual tenemos de objetivo para aplicar los steering behaviors")]
    public Hunter target;

    private Entity _food;
    private bool _foodOn;
    
    protected override void Start()
    {
        base.Start();
        AreaManager.foodOn += GetFood;
        target = GameObject.FindGameObjectWithTag("hunter").GetComponent<Hunter>();
    }

    private void Update()
    {
        var newForce = Vector3.zero;
        newForce = EvadeFunc(target.Position, target.Velocity, futureTime);
        AddForce(newForce);
        
    }
    
    private Vector3 Seek(Vector3 position)
    {
        // Calculamos la direccion al objetivo, la normalizamos y asumimos su velocidad maxima.
        var desiredDir = position - transform.position;
        desiredDir.Normalize();
        desiredDir *= _maxSpeed;

        // Calculamos la direccion entre la direccion deseado y nuestro velocity actual, luego lo multiplicamos por
        // la fuerza maxima del steering behavior.
        // Opcionalmente podemos normalizar el steering lo cual afectaria al steering maximo y seria mas consistente.
        var steering = desiredDir - _velocity;
        //steering.Normalize();
        steering *= maxSteeringForce;
        
        // Dibujamos la velocidad Antes de ser aplicado el steering behavior.
        Debug.DrawRay(transform.position, _velocity.normalized, Color.green);
        // Dibujamos la direccion deseada de steering behavior.
        Debug.DrawRay(transform.position, desiredDir.normalized, Color.red);
        // Dibujamos el steering behavior a aplicar.
        Debug.DrawRay(transform.position + _velocity.normalized, steering, Color.blue);
        
        // Devolvemos el steering behavior previamente escalado.
        return steering;
    }
    
    
    public Vector3 EvadeFunc(Vector3 position, Vector3 targetVelocity ,float time)
    {
        // El Evade es esencialmente calcular la direccion de un Pursuit, pero irnos para el lado contrario.
        return -Pursuit(position, targetVelocity, time);
    }
    
    private Vector3 Pursuit(Vector3 position, Vector3 targetVelocity ,float time)
    {
        // Calculamos la posicion en el futuro sumandole la velocidad por el tiempo a la posicion del objetivo.
        //var futurePosition = position + (targetVelocity * time);

        float distanceToTarget = Vector3.Distance(transform.position, position);
        float timeFactor = Mathf.Clamp01(distanceToTarget / time);
        float futureTime = time * timeFactor;

        Vector3 _futurePosition = position + targetVelocity * futureTime;

        // Devolvemos la direccion resultante de aplicar Seek a la posicion en el futuro.
        return Seek(_futurePosition);
    }
    
    
    private Vector3 Arrive(Vector3 position, float arrivalRadius)
    {
        // Calculamos la distancia entre nosotros y el objetivo.
        var distanceToTarget = Vector3.Distance(transform.position, position);

        // Si la distancia es mayor que el radio, calculamos Seek y devolvemos el vector.
        if (distanceToTarget > arrivalRadius)
            return Seek(position);
        
        // Si estamos dentro del radio, vamos a calcular la direccion opuesta deseada para poder ir frenando, la 
        // normalizamos, escalamos por la velocidad de movimiento maxima y por la distancia entre nosotros y el objetivo
        // respecto del radio. (Si estamos en el borde del radio va a ser 1f, si estamos en el objetivo sera 0f)
        var desired = position - transform.position;
        desired.Normalize();
        desired *= _maxSpeed * (distanceToTarget / arrivalRadius);

        // Una vez calculada la direccion deseada, calculamos el vector del steering y nos aseguramos que no exceda la
        // velocidad maxima.
        var steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, _maxSpeed);
        
        // Dibujamos la direccion de nuestro steering.
        Debug.DrawRay(transform.position, steering.normalized, Color.magenta);

        // Devolvemos el steering behavior.
        return steering;
    }

    private void GetFood()
    {
        if (_food == null)
        {
            _food = GameObject.FindGameObjectWithTag("food").GetComponent<Entity>();
            _foodOn = true;
        }
    }

    void OnDestroy()
    {
        AreaManager.foodOn -= GetFood;
    }
}
