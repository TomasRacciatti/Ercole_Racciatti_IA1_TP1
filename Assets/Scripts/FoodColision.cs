using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodColision : MonoBehaviour
{
    public float rayDistance = 2f;   // Distancia máxima del raycast
    public LayerMask pickUpLayer;  

    /*private void Start()
    {
        _areaManager = GameObject.Find("AreaManager").GetComponent<AreaManager>();
        if(_areaManager._foodColision == null) _areaManager._foodColision = this;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("food")) 
        {
            _areaManager._foodColision = null;
            Destroy(this.gameObject);
        }
    }
    
    private bool IsTargetVisible()
    {
        RaycastHit ObstacleHit;
        if (Objective)	// make sure we have an objective first or we get a dirty error.
            return (Physics.Raycast(OurShip.position, Objective.position - OurShip.position, out ObstacleHit, Mathf.Infinity) &&  ObstacleHit.transform != OurShip && ObstacleHit.transform == Objective);
        else
            return false;
    }*/
}
