using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestC : MonoBehaviour
{
    [Header("Walk,Run,Jump")]
    [SerializeField] private float WalkSpeed ;
    [SerializeField] private float RunSpeed;
    [SerializeField] private float JumpPower;
    [SerializeField] private float radius;
    [SerializeField] private Transform GroundPoint;
    [SerializeField] private LayerMask Ground;
    [HideInInspector] private bool CanWalk = true;
    [SerializeField] public bool isGrounded {get ; private set ;}
    public bool isRunning =false;
    public bool isJumping =false;

    [Header("WallSliding && Ledge Climp")]
    [SerializeField] private float WallCheckDistance;
    [SerializeField] private float WallSlidingSpeed;
    [SerializeField] private float WallJumpDirection;
    [SerializeField] private float WallJumpForceX;
    [SerializeField] private float WallJumpForceY;
    [SerializeField] private Transform CheckWall;
    [SerializeField] private LayerMask Wall;
    [SerializeField] private bool isWall;
    [SerializeField] private bool isWallSliding;
    private bool canWallJump;

    private bool isFacingRight = true;
    private float Hozion;
    private Rigidbody2D rb;
    private Animator Anim;
    private TL tl;
    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        tl = GetComponent<TL>();
    }
    private void Update() 
    { 
        if(CanWalk)
        {
            Walk();
        }
        if (isWallSliding)
        {
            JumpWall();
        }
        else if (isGrounded)
        {
            Jump();
        }
        Flip();

        Anim.SetBool("Run", Hozion !=0 && isGrounded);
        Anim.SetBool("Ground",isGrounded);
        Anim.SetFloat("Jump",rb.velocity.y);
    }
    private void FixedUpdate() 
    {
        CheckAll();
        CheckWallSliding();
        Anim.SetBool("WallSlide",isWallSliding);
    }
    private void Flip()
    {
        if((isFacingRight && Hozion < 0 )|| (!isFacingRight && Hozion >0))
        {
            isFacingRight = !isFacingRight;
            Vector3 face = transform.localScale;
            face.x = -face.x;
            transform.localScale = face;
        }
    }
    private void Walk()
    {
        Hozion = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(Hozion*WalkSpeed,rb.velocity.y);
        if(isWallSliding)
        {
            if(rb.velocity.y < -WallSlidingSpeed )
            {
                rb.velocity = new Vector2 (rb.velocity.x,-WallSlidingSpeed);
            }
        }
    }
    private void Jump()
    {   
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x,JumpPower);
        }
        else
        {
            isJumping = false;
        }
    }
    private void CheckAll()
    {
        isGrounded = Physics2D.OverlapCircle(GroundPoint.position,radius,Ground);
        Vector2 rayDirection = isFacingRight ? Vector2.right : Vector2.left;
        isWall = Physics2D.Raycast(CheckWall.position,rayDirection,WallCheckDistance,Wall);
        //isWall = Physics2D.Raycast(CheckWall.position,transform.right,WallCheckDistance,Ground);
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GroundPoint.position,radius);
        Vector3 gizmoDirection = isFacingRight ? Vector3.right : Vector3.left;
        //Gizmos.DrawLine(CheckWall.position,new Vector3(CheckWall.position.x + WallCheckDistance, CheckWall.position.y,CheckWall.position.z));
        Gizmos.DrawLine(CheckWall.position, CheckWall.position + gizmoDirection * WallCheckDistance);
    }
    private void CheckWallSliding()
    {
        if(isWall && !isGrounded && rb.velocity.y < 0)
        {
            isWallSliding = true;
            canWallJump = true;
        }
        else
        {
            isWallSliding = false;
            canWallJump = false;
        }
    }
    private void JumpWall()
    {
        if (canWallJump && Input.GetKeyDown(KeyCode.Space))
        {
            canWallJump = false;
            Vector2 ForceToAdd = new Vector2((isFacingRight ? -1 : 1) * WallJumpForceX ,WallJumpForceY);
            rb.velocity = ForceToAdd;
            Invoke("ResetWallJump", 0.1f); // Hoãn việc đặt lại canWallJump để đảm bảo nhân vật có đủ thời gian nhảy
        }
    }
    private void ResetWallJump()
    {
        canWallJump = true;
    }
}
