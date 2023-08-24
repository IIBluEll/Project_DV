using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugRay : MonoBehaviour
{
    public GameObject debugSphere;

    private void Update()
    {
        DebugRaycast();
    }

    private void DebugRaycast()
    {
        var ray = Camera.main.ViewportPointToRay(Vector2.one * .5f);

        var layerMask = 1 << LayerMask.NameToLayer("Player");
        layerMask = ~layerMask;
        
        if (Physics.Raycast(ray, out var hit, 50f, layerMask))
        {
            debugSphere.transform.position = hit.point;
        }
        else
        {
            debugSphere.transform.position = ray.origin + ray.direction * 50f;
        }
        
        Debug.DrawRay(ray.origin, ray.direction * 50f, Color.red);
    }
}
