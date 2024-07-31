using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Walk, Jump, Crouch")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float crouchWalkSpeed;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float currentSpeed1;

    private float horizontalInput;
    private float currentSpeed;
    private bool canFlip = true;
    private bool isFacingRight = true;
    private bool canRun = true;
    private bool isCrouching = false;
    private bool canDoubleJump = false;
    public bool isGrounded;

    [Header("Wall Slide and Jump")]
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float wallSlidingSpeed;
    [SerializeField] private Transform wallCheckPoint;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float wallJumpForceX;
    [SerializeField] private float wallJumpForceY;

    private bool isWallSliding;
    private bool isTouchingWall;
    //private bool isWallJumping;

    [Header("Slide")]
    [SerializeField] private float slideSpeed = 10f;
    [SerializeField] private float slideDuration = 0.5f;

    private float slideTime = 0f;
    public bool isSliding;

    [Header("Climb Ledge")]
    [SerializeField] private Transform ledgeCheckPoint;
    [SerializeField] private LayerMask ledgeLayer;
    [SerializeField] private Vector2 ledgeClimbOffset1;
    [SerializeField] private Vector2 ledgeClimbOffset2;

    private bool isTouchingLedge;
    private bool ledgeDetected;
    private bool canClimbLedge = false;
    private Vector2 ledgeBottomPos;
    private Vector2 ledgeClimbPos1;
    private Vector2 ledgeClimbPos2;

    private Animator animator;
    private Rigidbody2D rb;
    private PlayerStats playerStats;
    private ComboAttack comboAttack;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
        comboAttack = GetComponent<ComboAttack>();
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;
        if (canRun && !isSliding)
        {
            Move();
            HandleCrouch();
        }

        HandleSlide();
        HandleJump();
        FlipCharacter();
        CheckLedgeClimb();

        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        if (Time.timeScale == 0f) return;
        PerformChecks();
        if (!canClimbLedge)
        {
            CheckWallSliding();
        }
    }

    private void Move()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        currentSpeed = isCrouching ? crouchWalkSpeed : walkSpeed;
        rb.velocity = new Vector2(horizontalInput * currentSpeed, rb.velocity.y);

        if (isWallSliding && rb.velocity.y < -wallSlidingSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, -wallSlidingSpeed);
        }
    }

    private void FlipCharacter()
    {
        if (canFlip && ((isFacingRight && horizontalInput < 0) || (!isFacingRight && horizontalInput > 0)))
        {
            isFacingRight = !isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x = -scale.x;
            transform.localScale = scale;
        }
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isWallSliding)
            {
                PerformWallJump();
            }
            else if (isGrounded && !comboAttack.isAttacking)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                canDoubleJump = true;
            }
            else if (!isGrounded && canDoubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower * (2 / 3));
                canDoubleJump = false; // Chỉ nhảy đôi một lần
            }
        }
    }

    private void HandleSlide()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isSliding && isGrounded && playerStats.CurrentTL > playerStats.rateTLDown)
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
    }

    private void StartSlide()
    {
        isSliding = true;
        slideTime = 0;
        Vector2 slideDirection = isFacingRight ? Vector2.left : Vector2.right;
        rb.velocity = new Vector2(-slideDirection.x * slideSpeed, rb.velocity.y);
        animator.SetBool("Slide", isSliding);
    }

    private void EndSlide()
    {
        isSliding = false;
        animator.SetBool("Slide", isSliding);
    }

    private void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.S) && isGrounded)
        {
            isCrouching = !isCrouching;
            animator.SetBool("Crouch", isCrouching);
        }
    }

    private void PerformChecks()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
        Vector2 rayDirection = isFacingRight ? Vector2.right : Vector2.left;
        isTouchingWall = Physics2D.Raycast(wallCheckPoint.position, rayDirection, wallCheckDistance, wallLayer);
        isTouchingLedge = Physics2D.Raycast(ledgeCheckPoint.position, rayDirection, wallCheckDistance, ledgeLayer);

        if (isTouchingWall && !isTouchingLedge && !ledgeDetected)
        {
            ledgeDetected = true;
            ledgeBottomPos = wallCheckPoint.position;
        }
    }

    private void CheckWallSliding()
    {
        if (isTouchingWall && !isGrounded && rb.velocity.y < 0)
        {
            isWallSliding = true;
            //isWallJumping = true;
        }
        else
        {
            isWallSliding = false;
            //isWallJumping = false;
        }
    }

    private void PerformWallJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isTouchingWall)
        {
            //isWallJumping = false;
            Vector2 jumpForce = new Vector2((isFacingRight ? -1 : 1) * wallJumpForceX, wallJumpForceY);
            rb.velocity = jumpForce;
            //Invoke("ResetWallJump", 0.1f);
            canDoubleJump = false;
        }
    }

    // private void ResetWallJump()
    // {
    //     isWallJumping = true;
    // }

    private void CheckLedgeClimb()
    {
        if (ledgeDetected && !canClimbLedge)
        {
            canClimbLedge = true;
            ledgeClimbPos1 = isFacingRight
                ? ledgeBottomPos + new Vector2(wallCheckDistance - ledgeClimbOffset1.x, ledgeClimbOffset1.y)
                : ledgeBottomPos + new Vector2(-wallCheckDistance + ledgeClimbOffset1.x, ledgeClimbOffset1.y);

            ledgeClimbPos2 = isFacingRight
                ? ledgeBottomPos + new Vector2(wallCheckDistance + ledgeClimbOffset2.x, ledgeClimbOffset2.y)
                : ledgeBottomPos + new Vector2(-wallCheckDistance - ledgeClimbOffset2.x, ledgeClimbOffset2.y);

            canRun = false;
            canFlip = false;
            isWallSliding = false;
            animator.SetBool("canClimbLedge", canClimbLedge);
        }

        if (canClimbLedge)
        {
            transform.position = ledgeClimbPos1;
        }
    }

    public void FinishLedgeClimb()
    {
        canClimbLedge = false;
        transform.position = ledgeClimbPos2;
        canRun = true;
        canFlip = true;
        ledgeDetected = false;
        isWallSliding = true;
        animator.SetBool("canClimbLedge", canClimbLedge);
    }

    private void UpdateAnimations()
    {
        animator.SetBool("Slide", isSliding);
        animator.SetBool("Run", horizontalInput != 0 && isGrounded && !isCrouching);
        animator.SetBool("Ground", isGrounded);
        animator.SetFloat("Jump", rb.velocity.y);
        animator.SetBool("WallSlide", isWallSliding);
        animator.SetBool("CrouchWalk", isCrouching && horizontalInput != 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        Vector3 gizmoDirection = isFacingRight ? Vector3.right : Vector3.left;
        Gizmos.DrawLine(wallCheckPoint.position, wallCheckPoint.position + gizmoDirection * wallCheckDistance);
        Gizmos.DrawLine(ledgeCheckPoint.position, ledgeCheckPoint.position + gizmoDirection * wallCheckDistance);
    }
    //Biuld for Moblie
}
