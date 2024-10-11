using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodColision : MonoBehaviour
{
    public LayerMask pickUpLayer;
    public float castRadius;
    public float castDistance;
    public LayerMask targetMask;
    public Entity detectedObject;
    public int numRays;
    private void Update()
    {
        DetectCollision();
    }
    
    private void DetectCollision()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, castRadius, targetMask);
        
        foreach (var hitCollider in hitColliders)
        {
            AreaManager.foodOff();
            Destroy(gameObject);
        }
    }


}
