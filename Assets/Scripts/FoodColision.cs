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
        CheckForDestroy();
    }

    void CheckForDestroy()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, castRadius, transform.right, out hit, castDistance, targetMask))
        {
            Destroy(gameObject);
        }
        if (Physics.SphereCast(transform.position, castRadius, transform.forward, out hit, castDistance, targetMask))
        {
            Destroy(gameObject);
        }


    }


}
