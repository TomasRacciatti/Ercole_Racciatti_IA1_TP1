using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EvadeArrive : AgentBoid
{
    [Tooltip("Maxima fuerza aplicable por el steering behavior. (porcentual 1 = 100%)")]
    [Range(0f, 2f)] public float maxSteeringForce;

    [Tooltip("Tiempo en segundos que vamos a usar para predecir la posicion futura en Pursuit y Evade.")]
    public float futureTime = 1f;
    
    [Tooltip("Radio desde el cual empezamos a frenar al usar Arrive.")]
    public float arriveRadius = 1f;
    
    [Tooltip("Agente al cual tenemos de objetivo para aplicar los steering behaviors")]
    public Hunter target;

    public Entity food;
    private AreaManager _areaManager;
    public float arriveDistance;
    protected override void Start()
    {
        base.Start();
        AreaManager.foodOn += GetFood;
        AreaManager.foodOff += OutFood;
        target = GameObject.FindGameObjectWithTag("hunter").GetComponent<Hunter>();
        _areaManager = GameObject.FindGameObjectWithTag("areaManager").GetComponent<AreaManager>();
    }

    private void Update()
    {
        if (_areaManager._foodColision != null)
        {
            food = GameObject.FindGameObjectWithTag("food").GetComponent<Entity>();
            var ForceArrive = Vector3.zero;
            ForceArrive= Arrive(food.transform.position, arriveRadius);
            AddForce(ForceArrive);
        }

        var newForce = Vector3.zero;
        newForce = EvadeFunc(transform.position, target.Velocity, futureTime);
        AddForce(newForce);
        
    }
    
    private Vector3 Seek(Vector3 position)
    {
        var desiredDir = position - transform.position;
        desiredDir.Normalize();
        desiredDir *= _maxSpeed;
        
        var steering = desiredDir - _velocity;
        steering *= maxSteeringForce;
        
        Debug.DrawRay(transform.position, _velocity.normalized, Color.green);
        Debug.DrawRay(transform.position, desiredDir.normalized, Color.red);
        Debug.DrawRay(transform.position + _velocity.normalized, steering, Color.blue);

        return steering;
    }
    
    
    public Vector3 EvadeFunc(Vector3 position, Vector3 targetVelocity ,float time)
    {
        return -Pursuit(position, targetVelocity, time);
    }
    
    private Vector3 Pursuit(Vector3 position, Vector3 targetVelocity ,float time)
    {
        var futurePosition = position + (targetVelocity * time);
        
        return Seek(futurePosition);
    }
    
    
    private Vector3 Arrive(Vector3 position, float arrivalRadius)
    {
        Debug.Log("voy por la comida");
        var distanceToTarget = Vector3.Distance(transform.position, position);
        
        if (distanceToTarget > arrivalRadius)
            return Seek(position);
        
        var desired = position - transform.position;
        desired.Normalize();
        desired *= _maxSpeed * (distanceToTarget / arrivalRadius);
        
        var steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, _maxSpeed);
        
        Debug.DrawRay(transform.position, steering.normalized, Color.magenta);
        
        return steering;
    }

    private void GetFood()
    {
        if (food == null)
        {
            food = GameObject.FindGameObjectWithTag("food").GetComponent<Entity>();
        }
    }

    private void OutFood()
    {
        food = null;
    }

    void OnDestroy()
    {
        AreaManager.foodOn -= GetFood;
    }
}
