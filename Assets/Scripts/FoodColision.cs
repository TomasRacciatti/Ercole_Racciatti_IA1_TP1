using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodColision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.CompareTag("boid");
        Debug.Log("funca");
        Destroy(this.gameObject);
    }
}
