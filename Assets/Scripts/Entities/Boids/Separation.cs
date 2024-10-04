using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Separation : MonoBehaviour
{
    public static Vector3 SeparationFunc(AgentBoid agent, List<AgentBoid> boids, float radius, float maxSteeringForce)
    {
        // Creamos un vector para almacenar la direccion deseada.
        var desired = Vector3.zero;
        foreach (var boid in boids)
        {
            if (boid == agent) continue;
                
            // Calculamos la direccion desde nuestro boid al boid evaluado
            var dir = boid.transform.position - agent.transform.position;

            if (dir.magnitude <= radius)
            {
                // Calculamos una acumulacion lineal, cuando el boid este a una distancia casi igual al radio la division
                // sera casi 1, entonces el valor de la atenuacion sera casi 0. Caso contrario la division dara un valor
                // menor, por lo que el restante de la resta sera mas grande. Es decir, si esta cerca afectara mas, que
                // si esta lejos.
                var attenuation = 1 - (dir.sqrMagnitude / (radius * radius));
                    
                // multiplicamos la direccion por la atenuacion, y lo sumamos a la direccion deseada.
                desired -= dir * attenuation;
            }
        }

        // Si la direccion deseada es 0 la devolvemos, no hay nada mas que hacer.
        if (desired == Vector3.zero)
            return desired;

        desired.Normalize();
            
        // Calculamos el steering entre el vector deseado (escalado por maxSpeed) y nuestra velocity actual.
        var steering = Vector3.ClampMagnitude(desired * agent.MaxSpeed, agent.MaxSpeed) - agent.Velocity;
            
        // Clampeamos el steering por la fuerza maxima del steering
        steering = Vector3.ClampMagnitude(steering, maxSteeringForce);
            
        // devolvemos el steering
        return steering;
    }
}
