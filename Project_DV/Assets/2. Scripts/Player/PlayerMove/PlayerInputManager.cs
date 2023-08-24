using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    
    public float MouseX { get; private set; }
    public float MouseY { get; private set; }

    public bool IsSprinting { get; private set; }
    public bool IsAvoiding { get; private set; }
    
    public bool IsReload { get; private set; }

    public bool MouseButton0_Down { get; private set; }
    public bool MouseButton0_Up { get; private set; }
    public bool MouseButton1_Down { get; private set; }
    

    public event Action avoidAction;

    private void Update()
    {
        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");

        MouseX = Input.GetAxis("Mouse X");
        MouseY = Input.GetAxis("Mouse Y");

        IsSprinting = Input.GetKey(KeyCode.LeftShift);
        IsReload = Input.GetKeyDown(KeyCode.R);
        IsAvoiding = Input.GetKeyDown(KeyCode.Space);

        if (Input.GetKeyUp(KeyCode.Space))
        {
            IsAvoiding = true;
            avoidAction?.Invoke();
        }
        else
        {
            IsAvoiding = false;
        }

        MouseButton0_Down = Input.GetMouseButtonDown(0);
        MouseButton0_Up = Input.GetMouseButtonUp(0);
        MouseButton1_Down = Input.GetMouseButton(1);

    }
}
