using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationControl : MonoBehaviour
{
    private Animator playerAnimator;
    private PlayerInputManager playerInputMgr;

    public float DirX
    {
        get => playerAnimator.GetFloat("DirX");
        set => playerAnimator.SetFloat("DirX",value);
    }
   
    public float DirY
    {
        get => playerAnimator.GetFloat("DirY");
        set => playerAnimator.SetFloat("DirY",value);
    }
    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerInputMgr = GetComponent<PlayerInputManager>();
    }

    private void Update()
    {
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Move"))
        {
            DirX = playerInputMgr.Horizontal;
            DirY = playerInputMgr.Vertical;    
        }
    }

    public void PlayAnim(string stateName, int layer, float normalizedTime)
    {
        playerAnimator.Play(stateName, layer, normalizedTime);
    }
    
}
