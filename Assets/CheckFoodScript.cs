using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFoodScript : MonoBehaviour
{
    public float rayDistance = 2f;
    public LayerMask pickUpLayer;
    
    private void Update()
    {
        CheckForFood();
    }
    void CheckForFood()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayDistance, pickUpLayer))
        {
            if (hit.transform != null)
            {
                Destroy(hit.transform.gameObject);
            }
        }
    }
    

}
