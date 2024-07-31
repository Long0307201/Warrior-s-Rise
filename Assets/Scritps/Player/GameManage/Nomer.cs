using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nomer : MonoBehaviour
{
    [Header("Walk,Jump,Crouch")]
    [SerializeField] private float SpeedWalk;
    [SerializeField] private float PowerJump;
    [SerializeField] private float SpeedWalkCrouch;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask GroundLayer;
    [SerializeField] private Transform PointCheckGround;
    [SerializeField] private float Hozion;
    private float currentSpeed;
    private bool canFlip = true;
    private bool isFacingRight = true;
    private bool CanRun = true;
    //public bool canJump = true;
    public bool isGround;
    public bool isJumping = false;
    //public bool DoubleJump;
    private bool isCrouching = false;
    [Header("Wall_Slide , JumpWall")]
    [SerializeField] private float WallCheckDistance;
    [SerializeField] private float WallSlidingSpeed;
    [SerializeField] private Transform PointCheckWall;
    [SerializeField] private LayerMask WallLayer;
    [SerializeField] private float WallJumpForceX;
    [SerializeField] private float WallJumpForceY;
    [SerializeField] private bool isWallSliding;
    [SerializeField] private bool isWall;
    private bool isWallJumping;
    [Header("Slide")]
    [SerializeField] private float slideSpeed = 10f;
    [SerializeField] private float slideDuration = 0.5f;
    [SerializeField] private float slideTime = 0f;
    public bool isSliding;
    [Header("Climp Ledge")]
    [SerializeField] private Transform ledgeCheck;
    [SerializeField] private LayerMask LedgeLayer;
    [SerializeField] private bool isTouchingLedge;
    [SerializeField] private bool ledgeDetected;
    [SerializeField] private bool canClimbLedge = false;
    [SerializeField] private Vector2 Offset1;
    [SerializeField] private Vector2 Offset2;
    private Vector2 ledgePosBot;
    private Vector2 ledgePos1;
    private Vector2 ledgePos2;
    private Animator Anim;
    private Rigidbody2D rb;
    private PlayerStats playerStats;
    private ComboAttack comboAttack;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        comboAttack = GetComponent<ComboAttack>();
        playerStats = GetComponent<PlayerStats>();
    }
    private void Update()
    {
        if (CanRun && !isSliding)
        {
            Run();
            WalkCrouch();
        }
        //Slide
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isSliding && isGround && !isSliding && playerStats.CurrentTL > playerStats.rateTLDown)
        {
            StartSlide();
        }
        if (isSliding)
        {
            slideTime += Time.deltaTime;
            if (slideTime >= slideDuration)
            {
                EndSlide();
            }
        }
        //Jump && WallJump
        if (isWallSliding)
        {
            JumpWall();
        }
        else if (isGround && !comboAttack.isAttacking)
        {
            Jump();
        }
        Flip();
        CheckLedgeClimb();
        //Set anim
        Anim.SetBool("Slide", isSliding);
        Anim.SetBool("Run", Hozion != 0 && isGround && !isCrouching);
        Anim.SetBool("Ground", isGround);
        Anim.SetFloat("Jump", rb.velocity.y);
    }
    private void FixedUpdate()
    {
        CheckAll();
        if (!canClimbLedge)
        {
            CheckWallSliding();
        }
        Anim.SetBool("WallSlide", isWallSliding);
    }

    private void Run()
    {
        Hozion = Input.GetAxis("Horizontal");
        currentSpeed = isCrouching ? SpeedWalkCrouch : SpeedWalk;
        rb.velocity = new Vector2(Hozion * currentSpeed, rb.velocity.y);
        if (isWallSliding)
        {
            if (rb.velocity.y < -WallSlidingSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -WallSlidingSpeed);
            }
        }
        Anim.SetBool("CrouchWalk", isCrouching && Hozion != 0);
    }
    private void Flip()
    {
        if (canFlip)
        {
            if ((isFacingRight && Hozion < 0) || (!isFacingRight && Hozion > 0))
            {
                isFacingRight = !isFacingRight;
                Vector3 face = transform.localScale;
                face.x = -face.x;
                transform.localScale = face;
            }
        }
    }
    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, PowerJump);
        }

    }
    private void CheckAll()
    {
        isGround = Physics2D.OverlapCircle(PointCheckGround.position, radius, GroundLayer);
        Vector2 rayDirection = isFacingRight ? Vector2.right : Vector2.left;
        isWall = Physics2D.Raycast(PointCheckWall.position, rayDirection, WallCheckDistance, WallLayer);
        // Physics2D.Raycast(Điểm check, vector,độ dài vector , layermask)
        isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, rayDirection, WallCheckDistance, LedgeLayer);

        if (isWall && !isTouchingLedge && !ledgeDetected)
        {
            ledgeDetected = true;
            ledgePosBot = PointCheckWall.position;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(PointCheckGround.position, radius);
        Vector3 gizmoDirection = isFacingRight ? Vector3.right : Vector3.left;
        Gizmos.DrawLine(PointCheckWall.position, PointCheckWall.position + gizmoDirection * WallCheckDistance);
        Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + gizmoDirection * WallCheckDistance);
    }
    private void WalkCrouch()
    {
        if (Input.GetKeyDown(KeyCode.S) && isGround)
        {
            isCrouching = !isCrouching;
            Anim.SetBool("Crouch", isCrouching);
            if (isCrouching)
            {
                Anim.SetBool("Run", false);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ham"))
        {
            isCrouching = true;
            WalkCrouch();
        }
        else
        {
            isCrouching = false;
        }
    }
    private void CheckWallSliding()
    {
        if (isWall && !isGround && rb.velocity.y < 0)
        {
            isWallSliding = true;
            isWallJumping = true;
        }
        else
        {
            isWallSliding = false;
            isWallJumping = false;
        }
    }
    private void JumpWall()
    {
        if (isWallJumping && Input.GetKeyDown(KeyCode.Space))
        {
            isWallJumping = false;
            Vector2 ForceToAdd = new Vector2((isFacingRight ? -1 : 1) * WallJumpForceX, WallJumpForceY);
            rb.velocity = ForceToAdd;
            Invoke("ResetWallJump", 0.1f); // Hoãn việc đặt lại isWallJumping để đảm bảo nhân vật có đủ thời gian nhảy
        }
    }
    private void ResetWallJump()
    {
        isWallJumping = true;
    }
    private void StartSlide()
    {
        isSliding = true;
        slideTime = 0;
        Vector2 SlideVector = isFacingRight ? Vector2.left : Vector2.right;
        //rb.AddForce(SlideVector *PowerSlide);
        rb.velocity = new Vector2(-SlideVector.x * slideSpeed, rb.velocity.y);
        Anim.SetBool("Slide", isSliding);
    }
    private void EndSlide()
    {
        isSliding = false;
        Anim.SetBool("Slide", isSliding);
    }
    //Climp Ledge
    private void CheckLedgeClimb()
    {
        if (ledgeDetected && !canClimbLedge)
        {
            canClimbLedge = true;
            if (isFacingRight)
            {
                ledgePos1 = ledgePosBot + new Vector2(WallCheckDistance - Offset1.x, Offset1.y);
                ledgePos2 = ledgePosBot + new Vector2(WallCheckDistance + Offset2.x, Offset2.y);
            }
            else
            {
                ledgePos1 = ledgePosBot + new Vector2(-WallCheckDistance + Offset1.x, Offset1.y);
                ledgePos2 = ledgePosBot + new Vector2(-WallCheckDistance - Offset2.x, Offset2.y);
            }
            CanRun = false;
            canFlip = false;
            isWallSliding = false;
            Anim.SetBool("canClimbLedge", canClimbLedge);
        }
        if (canClimbLedge)
        {
            transform.position = ledgePos1;
        }
    }

    public void FinishLedgeClimb()
    {
        canClimbLedge = false;

        transform.position = ledgePos2;
        CanRun = true;
        canFlip = true;
        ledgeDetected = false;
        isWallSliding = true;
        Anim.SetBool("canClimbLedge", canClimbLedge);
    }
}
