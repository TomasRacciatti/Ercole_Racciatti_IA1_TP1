using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase estatica con funciones de Steering Behaviors. Usa Agent y Entity como las clases base para calcular.
/// </summary>
public static class SteeringBoid
{
    public static bool ShowDebugSteering { get; set; } = false;
        
    /// <summary>
    /// Seek es un steering behavior el cual se usa para ir en direction a un objetivo gradualmente.
    /// </summary>
    /// <param name="agent">Agente que va a aplicar el steering</param>
    /// <param name="target">Objetivo sobre el cual calcular el steering</param>
    /// <param name="maxSteeringForce">Maxima fuerza que puede aplicar el steering.</param>
    /// <returns>Devuelve la direction que deberemos sumar para llegar al destino siguiendo el steering.</returns>
    public static Vector3 Seek(AgentBoid agent, Entity target, float maxSteeringForce)
    {
        return Seek(agent.Position, agent.Velocity, agent.MaxSpeed, target.GetClosestPosition(agent.Position), maxSteeringForce);
    }

    /// <summary>
    /// Seek es un steering behavior el cual se usa para ir en direction a un objetivo gradualmente.
    /// </summary>
    /// <param name="agentPosition">Posicion del agente que va a aplicar el steering</param>
    /// <param name="agentVelocity">Velocidad del agente que va a aplicar el steering</param>
    /// <param name="agentMaxSpeed">Velocidad maxima del agente que va a aplicar el steering</param>
    /// <param name="targetPosition">Posicion del Objetivo sobre el cual calcular el steering</param>
    /// <param name="maxSteeringForce">Maxima fuerza que puede aplicar el steering.</param>
    /// <returns>Devuelve la direction que deberemos sumar para llegar al destino siguiendo el steering.</returns>
    public static Vector3 Seek(Vector3 agentPosition, Vector3 agentVelocity, float agentMaxSpeed, Vector3 targetPosition, float maxSteeringForce)
    {
        // Calculamos la direccion al objetivo, la normalizamos y asumimos su velocidad maxima.
        var desiredDir = targetPosition - agentPosition;
        desiredDir.Normalize();
        desiredDir *= agentMaxSpeed;

        // Calculamos la direccion entre la direccion deseado y nuestro velocity actual, luego lo multiplicamos por
        // la fuerza maxima del steering behavior.
        // Opcionalmente podemos normalizar el steering lo cual afectaria al steering maximo y seria mas consistente.
        var steering = desiredDir - agentVelocity;
        //steering.Normalize();
        steering *= maxSteeringForce;

        if (ShowDebugSteering)
        {
            // Dibujamos la velocidad Antes de ser aplicado el steering behavior.
            Debug.DrawRay(agentPosition, agentVelocity.normalized, Color.green);
            // Dibujamos la direccion deseada de steering behavior.
            Debug.DrawRay(agentPosition, desiredDir.normalized, Color.red);
            // Dibujamos el steering behavior a aplicar.
            Debug.DrawRay(agentPosition + agentVelocity.normalized, steering, Color.blue);
        }

        // Devolvemos el steering behavior previamente escalado.
        return steering;
    }
        
    /// <summary>
    /// Flee es un steering behavior el cual se usa para ir en direccion contraria a un objetivo.
    /// </summary>
    /// <param name="agent">Agente que va a aplicar el steering</param>
    /// <param name="target">Objetivo sobre el cual calcular el steering</param>
    /// <param name="maxSteeringForce">Maxima fuerza que puede aplicar el steering.</param>
    /// <returns>Devuelve la direccion que deberemos sumar para llegar al destino siguiendo el steering.</returns>
    public static Vector3 Flee(AgentBoid agent, Entity target, float maxSteeringForce)
    {
        // El Flee es esencialmente calcular la direccion de un Seek, pero irnos para el lado contrario.
        return -Seek(agent, target, maxSteeringForce);
    }

    /// <summary>
    /// Pursuit es un steering behavior el cual nos permite calcular la direccion a un objetivo en una posicion futura,
    /// basado en como se esta moviendo actualmente. Esto nos permitira predecir donde va a estar y asi poder
    /// interceptarlo. Un uso comun, ademas del movimiento, es calcular la direccion de una bala para intentar acertar
    /// al objetivo (e.j: Arqueros en el 'Age of empires II' cuando se desarrolla 'Balistica' usan Pursuit para apuntar)
    /// </summary>
    /// <param name="agent">Agente que va a aplicar el steering.</param>
    /// <param name="target">Objetivo sobre el cual calcular el steering.</param>
    /// <param name="maxSteeringForce">Maxima fuerza que puede aplicar el steering.</param>
    /// <param name="predictionTime">Tiempo en el futuro donde haremos la prediccion.</param>
    /// <returns>Devuelve la direccion que deberemos sumar para llegar al destino siguiendo el steering.</returns>
    public static Vector3 Pursuit(AgentBoid agent, Entity target, float maxSteeringForce, float predictionTime)
    {
        return Pursuit(agent, target.GetClosestPosition(agent.Position), target.Velocity, maxSteeringForce, predictionTime);
    }
        
    /// <summary>
    /// Pursuit es un steering behavior el cual nos permite calcular la direccion a un objetivo en una posicion futura,
    /// basado en como se esta moviendo actualmente. Esto nos permitira predecir donde va a estar y asi poder
    /// interceptarlo. Un uso comun, ademas del movimiento, es calcular la direccion de una bala para intentar acertar
    /// al objetivo (e.j: Arqueros en el 'Age of empires II' cuando se desarrolla 'Balistica' usan Pursuit para apuntar)
    /// </summary>
    /// <param name="agent">Agente que va a aplicar el steering.</param>
    /// <param name="targetPosition">Posicion del objetivo.</param>
    /// <param name="targetVelocity">Velocidad del objetivo</param>
    /// <param name="maxSteeringForce">Maxima fuerza que puede aplicar el steering.</param>
    /// <param name="predictionTime">Tiempo en el futuro donde haremos la prediccion.</param>
    /// <returns></returns>
    public static Vector3 Pursuit(AgentBoid agent, Vector3 targetPosition, Vector3 targetVelocity, float maxSteeringForce, float predictionTime)
    {
        // Calculamos la posicion en el futuro sumandole la velocidad por el tiempo a la posicion del objetivo.
        var futurePosition = targetPosition + (targetVelocity * predictionTime);

        return Pursuit(agent, futurePosition, maxSteeringForce);
    }
        
    /// <summary>
    /// Simplified version of Pursuit.
    /// Pursuit es un steering behavior el cual nos permite calcular la direccion a un objetivo en una posicion futura,
    /// basado en como se esta moviendo actualmente. Esto nos permitira predecir donde va a estar y asi poder
    /// interceptarlo. Un uso comun, ademas del movimiento, es calcular la direccion de una bala para intentar acertar
    /// al objetivo (e.j: Arqueros en el 'Age of empires II' cuando se desarrolla 'Balistica' usan Pursuit para apuntar)
    /// </summary>
    /// <param name="agent">Agente que va a aplicar el steering.</param>
    /// <param name="futurePosition">Posicion futura a la que ir.</param>
    /// <param name="maxSteeringForce">Maxima fuerza que puede aplicar el steering.</param>
    /// <returns></returns>
    public static Vector3 Pursuit(AgentBoid agent, Vector3 futurePosition, float maxSteeringForce)
    {
        // Devolvemos la direccion resultante de aplicar Seek a la posicion en el futuro.
        return Seek(agent.Position, agent.Velocity, agent.MaxSpeed, futurePosition, maxSteeringForce);
    }

    /// <summary>
    /// Evade es un steering behavior el cual nos permite calcular la direccion a un objetivo en una posicion futura,
    /// basado en como se esta moviendo actualmente. Esto nos permitira predecir donde va a estar y asi poder evitar
    /// esa direccion e intentar alejarnos de ella. Es el contrario de Pursuit.
    /// </summary>
    /// <param name="agent">Agente que va a aplicar el steering.</param>
    /// <param name="target">Objetivo sobre el cual calcular el steering.</param>
    /// <param name="maxSteeringForce">Maxima fuerza que puede aplicar el steering.</param>
    /// <param name="predictionTime">Tiempo en el futuro donde haremos la prediccion.</param>
    /// <returns>Devuelve la direccion que deberemos sumar para escapar del objetivo siguiendo el steering.</returns>
    public static Vector3 Evade(AgentBoid agent, Entity target, float maxSteeringForce, float predictionTime)
    {
        // El Evade es esencialmente calcular la direccion de un Pursuit, pero irnos para el lado contrario.
        return -Pursuit(agent, target, maxSteeringForce, predictionTime);
    }
    
    /// <summary>
    /// Simplified version of Evade.
    /// Evade es un steering behavior el cual nos permite calcular la direccion a un objetivo en una posicion futura,
    /// basado en como se esta moviendo actualmente. Esto nos permitira predecir donde va a estar y asi poder evitar
    /// esa direccion e intentar alejarnos de ella. Es el contrario de Pursuit.
    /// </summary>
    /// <param name="agent">Agente que va a aplicar el steering.</param>
    /// <param name="futurePosition">Posicion futura a la que ir.</param>
    /// <param name="maxSteeringForce">Maxima fuerza que puede aplicar el steering.</param>
    /// <returns>Devuelve la direccion que deberemos sumar para escapar del objetivo siguiendo el steering.</returns>
    public static Vector3 Evade(AgentBoid agent, Vector3 futurePosition, float maxSteeringForce)
    {
        // El Evade es esencialmente calcular la direccion de un Pursuit, pero irnos para el lado contrario.
        return -Pursuit(agent, futurePosition, maxSteeringForce);
    }
        
    /// <summary>
    /// Arrive es un steering behavior el cual nos permite ir hacia un objetivo (aplicando Seek) hasta estar dentro de
    /// un radio determinado, en el cual iremos frenando hasta llegar al objetivo deseado. En dicho momento la velocity
    /// debera ser 0.
    /// </summary>
    /// <param name="agent">Agente que va a aplicar el steering.</param>
    /// <param name="target">Objetivo sobre el cual calcular el steering.</param>
    /// <param name="maxSteeringForce">Maxima fuerza que puede aplicar el steering.</param>
    /// <param name="arrivalRadius">Radio de la distancia apartir de la cual quiero empezar a frenar.</param>
    /// <returns>Devuelve la direccion que deberemos sumar para llegar a destino siguiendo el steering.</returns>
    public static Vector3 Arrive(AgentBoid agent, Entity target, float maxSteeringForce, float arrivalRadius)
    {
        var targetPosition = target.GetClosestPosition(agent.Position);
        // Calculamos la distancia entre nosotros y el objetivo.
        var distanceToTarget = Vector3.Distance(agent.Position, targetPosition);

        // Si la distancia es mayor que el radio, calculamos Seek y devolvemos el vector.
        if (distanceToTarget > arrivalRadius)
            return Seek(agent, target, maxSteeringForce);

        // Si estamos dentro del radio, vamos a calcular la direccion opuesta deseada para poder ir frenando, la 
        // normalizamos, escalamos por la velocidad de movimiento maxima y por la distancia entre nosotros y el objetivo
        // respecto del radio. (Si estamos en el borde del radio va a ser 1f, si estamos en el objetivo sera 0f)
        var desired = targetPosition - agent.Position;
        desired.Normalize();
        desired *= agent.MaxSpeed * (distanceToTarget / arrivalRadius);

        // Una vez calculada la direccion deseada, calculamos el vector del steering y nos aseguramos que no exceda la
        // velocidad maxima.
        var steering = desired - agent.Velocity;
        steering = Vector3.ClampMagnitude(steering, agent.MaxSpeed);

        if (ShowDebugSteering)
        {
            // Dibujamos la direccion de nuestro steering.
            Debug.DrawRay(agent.Position, steering.normalized, Color.magenta);
        }

        // Devolvemos el steering behavior.
        return steering;
    }
        
    /// <summary>
    /// Calculamos un vector de Avoidance basados en si hay un obstaculo de un determinado layer en la posicion 
    /// mas proxima hacia delante del agente a evaluar.
    /// </summary>
    /// <param name="agent">Agente que va a aplicar el steering.</param>
    /// <param name="maxSteeringStrength">Maxima fuerza que puede aplicar el steering.</param>
    /// <param name="castRadius">Radius del SphereCast, similar al tamaño del agente.</param>
    /// <param name="aheadDistance">Distancia a futuro que vamos a chequear por obstaculos.</param>
    /// <param name="obstacleMask">Mascara para comparar layer de colision de los objetos obstaculo.</param>
    /// <param name="avoidanceAngle">Angulo que se rotara la direccion para calcular el steering.</param>
    /// <returns>Devuelve la direccion que deberemos sumar para evitar el obstaculo siguiendo el steering.</returns>
    public static Vector3 ObstacleAvoidance(AgentBoid agent, float maxSteeringStrength, float castRadius, float aheadDistance, LayerMask obstacleMask, float avoidanceAngle = 90)
    {
        if (Physics.SphereCast(agent.Position, castRadius, agent.Velocity, out var hit, aheadDistance, obstacleMask))
        {
            var obstacle = hit.transform;
            var dirToObject = obstacle.position - agent.Position;
            var angleInBetween = Vector3.SignedAngle(agent.Velocity, dirToObject, Vector3.up);

            var rotationValue = angleInBetween >= 0 ? -avoidanceAngle : avoidanceAngle;
            var desiredDir = Quaternion.Euler(0, rotationValue, 0) * agent.Velocity;
            desiredDir.Normalize();
            desiredDir *= agent.MaxSpeed;
                
            var steering = desiredDir - agent.Velocity;
            steering *= maxSteeringStrength;

            Debug.DrawRay(agent.Position, steering.normalized, Color.magenta);
                
            return steering;
        }

        return Vector3.zero;
    }
        
    /// <summary>
    /// Calculamos la separacion deseada entre los boids. El objetivo es que nunca esten mas cerca los unos a los otros
    /// que el radio definido en esta funcion.
    /// </summary>
    /// <param name="agent">Agente que va a aplicar el steering.</param>
    /// <param name="boids">Lista de boids que analizar.</param>
    /// <param name="radius">Radio en el que considerar para la separacion.</param>
    /// <param name="maxSteeringForce">Maxima fuerza que puede aplicar el steering.</param>
    /// <returns>Devuelve la direccion que deberemos sumar para alejarnos de los elementos dentro del radio.</returns>
    public static Vector3 Separation(AgentBoid agent, List<Entity> boids, float radius, float maxSteeringForce)
    {
        // Creamos un vector para almacenar la direccion deseada.
        var desired = Vector3.zero;
        foreach (var boid in boids)
        {
            if (boid == agent) continue;
                
            // Calculamos la direccion desde nuestro boid al boid evaluado
            var dir = boid.transform.position - agent.Position;

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

    /// <summary>
    /// Calculamos el vector deseado para que las velocidades entre los neighbours esten alineadas, es decir, que a
    /// traves del tiempo sus direcciones se parezcan mas y mas.
    /// </summary>
    /// <param name="agent">Agente que va a aplicar el steering.</param>
    /// <param name="boids">Lista de boids que analizar.</param>
    /// <param name="maxSteeringForce">Maxima fuerza que puede aplicar el steering.</param>
    /// <returns>Devuelve la direccion deseada para estar alineados a nuestros vecinos.</returns>
    public static Vector3 Alignment(AgentBoid agent, List<Entity> boids, float maxSteeringForce)
    {
        // Creamos un vector para almacenar la direccion deseada.
        var desired = Vector3.zero;
        foreach (var boid in boids)
        {
            // Sumamos la velocity del boid normalizada al acumulado deseado
            desired += boid.Velocity.normalized;
        }
            
        // Normalizamos el vector y lo comparamos con nuestro velocity. Si son iguales no hay nada que hacer, devolvemos 0.
        desired.Normalize();
            
        if(desired == agent.transform.forward)
            return Vector3.zero;

        // Escalamos el vector a la velocidad maxima y calculamos el steering. Clampeamos a la fuerza maxima.
        var steering = (desired * agent.MaxSpeed) - agent.Velocity;
        steering = Vector3.ClampMagnitude(steering, maxSteeringForce);
            
        // devolvemos el steering
        return steering;
    }

    /// <summary>
    /// Calculamos el vector deseado para acercarnos a la posicion promedio de todos los neighbours dentro de un radio.
    /// En esencia nos mantenemos cerca entre todos.
    /// </summary>
    /// <param name="agent">Agente que va a aplicar el steering.</param>
    /// <param name="boids">Lista de boids que analizar.</param>
    /// <param name="maxSteeringForce">Maxima fuerza que puede aplicar el steering.</param>
    /// <param name="debugLocation">Debug variable to draw on gizmo.</param>
    /// <returns>Devuelve la direccion deseada para movernos al promedio de posiciones de los boids encontrados.</returns>
    public static Vector3 Cohesion(AgentBoid agent, List<Entity> boids, float maxSteeringForce, out Vector3 debugLocation)
    {
        // para calcular el vector deseado queremos sumar todas las posiciones y dividirlas por la cantidad de boids cercanos.
        Vector3 desired = Vector3.zero;
        int count = 0;
        foreach (var boid in boids)
        {
            desired += boid.transform.position;
            count++; 
        }
            
        // Si el agregado es igual a nuestra posicion, entonces somos lo unico detectado, devolvemos 0.
        if (count <= 1)
        {
            debugLocation = agent.Position;
            return Vector3.zero;
        }

        // Calculamos la posicion promedio dividiendo el agregado por la cantidad de posiciones que contiene.
        var averagePos = desired / count;

        debugLocation = averagePos;
            
        // Calculamos la direccion hacia la posicion promedio, la normalizamos y escalamos.
        var averagePosDir = averagePos - agent.Position;
        averagePosDir.Normalize();
        averagePosDir *= agent.MaxSpeed;

        // Con esta direccion calculamos el steering, clampeandolo a la fuerza maxima.
        var steering = averagePosDir - agent.Velocity;
        steering = Vector3.ClampMagnitude(steering, maxSteeringForce);
            
        // devolvemos el steering
        return steering;
    }
}