using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDebugRay : MonoBehaviour
{
    public GameObject debugSphere;

    private void Update()
    {
        DebugRaycast();
    }

    private void DebugRaycast()
    {
        Debug.DrawRay(transform.position, transform.forward * 50f, Color.green);
    }
}
