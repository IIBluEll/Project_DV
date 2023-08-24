using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerInputManager inputMgr;
    private PlayerStatusManager statusMgr;
    private PlayerAnimationControl animControl;

    private float horizontalMove;
    private float verticalMove;

    private Vector3 moveDirection;
    private Vector3 slopeMoveDirection;
    
    private RaycastHit slopeHit;                            // 경사면 감지 정보

    [Header("## Movement ##")] 
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float movementMulti = 10f;         // 움직임 배율
    [SerializeField] private float airMulti = 0.4f;             // 공중 움직임 배율
    
    [Space(10f), Header(" Sprinting ")]
    [SerializeField] private float acceleration = 10f;      // 가속도

    [Space(10f), Header(" Drag ")] 
    [SerializeField] private float groundDrag = 6f;         // 지면 마찰력
    [SerializeField] private float airDrag = 2f;            // 공중 마찰력
    
    [Space(10f), Header(" Ground Detection")] 
    [SerializeField] private LayerMask groundMask;          // 바닥 감지 레이어 마스크
    [SerializeField] private Transform groundCheck;         // 바닥 감지 위치
    [SerializeField] private bool isGrounded;               // 바닥에 닿았는지?
                     private float groundDistance = 0.1f;   // 바닥과의 거리
                     private float playerHeight = 2f;       // 플레이어 높이

                     public bool isPlayerDie;
                     public bool isAvoiding;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputMgr = GetComponent<PlayerInputManager>();
        statusMgr = GetComponent<PlayerStatusManager>();
        animControl = GetComponent<PlayerAnimationControl>();
        
        rb.freezeRotation = true;

        StartCoroutine("CheckGround");
    }

    private void Start()
    {
        inputMgr.avoidAction += AvoidAction;
    }

    private void Update()
    {
        if (!isAvoiding)
        {
            moveDirection = 
                transform.forward * inputMgr.Vertical + transform.right * inputMgr.Horizontal;
        }
        
        // 경사면에서의 움직임 방향 계산
        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
        
        ControlSpeed();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    // 경사면 위에 있는지 검사하는 메서드
    private bool OnSlope()
    {
        // 아래 방향으로 레이를 쏘아 경사면 감지를 시도함
        if (!Physics.Raycast(transform.position, Vector3.down, out slopeHit, 0.1f)) 
            return false;

        // 경사면의 normal 벡터가 up이 아니라면 경사면이라고 판단
        return slopeHit.normal != Vector3.up;
    }

    // 플레이어 속도 제어를 하는 메서드
    private void ControlSpeed()
    {
        var targetSpeed = 0f;

        // 달리기 상태이고 바닥에 있을 때
        if (inputMgr.IsSprinting && isGrounded)
        {
            targetSpeed = statusMgr.Run_Speed;
        }
        // 움직이지 않을 때
        else if (inputMgr.Horizontal == 0 && inputMgr.Vertical == 0)
        {
            targetSpeed = 0;
        }
        // 걷는 상태이고 바닥에 있을 때 
        else
        {
            targetSpeed = statusMgr.Walk_Speed;
        }
        
        // 속도에 차이가 있을 때만 Lerp를 사용해 속도 변경
        if (Mathf.Abs(moveSpeed - targetSpeed) > 0.05f)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, acceleration * Time.deltaTime);
        }
    }

    // 플레이어가 움직이는 메서드
    private void MovePlayer()
    {
        Vector3 forceDirection;

        var onSlope = OnSlope();

        switch (isGrounded)
        {
            // 바닥에 닿고 있으며 경사면이 아닐 때
            case true when !onSlope:
                forceDirection = moveDirection;
                rb.drag = groundDrag;
                break;
            
            // 바닥에 닿고 있으며 경사면일 때
            case true when onSlope:
                forceDirection = slopeMoveDirection;
                rb.drag = groundDrag;
                break;
            
            // 공중에 있을 때
            default:
                forceDirection = moveDirection;
                rb.drag = airDrag;
                break;
        }

        var forceMultiplier = isGrounded ? movementMulti : movementMulti * airMulti;

        rb.AddForce(forceDirection.normalized * moveSpeed * forceMultiplier, ForceMode.Acceleration);
    }

    private void AvoidAction()
    {
        if (isAvoiding || (inputMgr.Horizontal == 0 && inputMgr.Vertical == 0))
            return;

        StartCoroutine("AvoidCoroutine");
    }

    private IEnumerator AvoidCoroutine()
    {
        isAvoiding = true;
        
        animControl.PlayAnim("Roll",0,0);

        yield return new WaitForSeconds(1f);

        isAvoiding = false;
    }
    
    // 바닥 감지 확인
    private IEnumerator CheckGround()
    {
        while (true)
        {
            if (isPlayerDie)
                yield break;
            
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            yield return new WaitForSeconds(.1f);
        }
    }

}
