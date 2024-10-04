using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodColision : MonoBehaviour
{
    private LayerMask mask;
    private AreaManager _areaManager;

    private void Start()
    {
        _areaManager = GameObject.Find("AreaManager").GetComponent<AreaManager>();
        if(_areaManager._foodColision == null) _areaManager._foodColision = this;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer ("food")) 
        {
            _areaManager._foodColision = null;
            Destroy(this.gameObject);
        }
    }
}
