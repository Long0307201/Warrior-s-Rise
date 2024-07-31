using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class AttakPlayer : MonoBehaviour
{

    public Transform[] patrolPoints;
    public float speed = 2f;
    public float detectionRadius = 5f;
    public Transform player;
    private int currentPatrolIndex;
    private bool isChasingPlayer;
    private Rigidbody2D rb;
    private Vector3 lastPosition;
    [SerializeField] private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPatrolIndex = 0;
        lastPosition = transform.position;
    }

    void Update()
    {
        if (player != null)
        {
            if (Vector3.Distance(transform.position, player.position) <= detectionRadius)
            {
                isChasingPlayer = true;
            }
            else
            {
                isChasingPlayer = false;
            }

            if (isChasingPlayer)
            {
                ChasePlayer();
            }
            else
            {
                Patrol();
            }
        }
        else
        {
            // Nếu player null, có thể dừng hành vi hoặc thêm logic tìm lại player
            isChasingPlayer = false;
            Patrol(); // Hoặc hành vi mặc định khác khi không tìm thấy player
        }

        //anim.SetBool("Attack", isChasingPlayer);
        FlipSprite();
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        Vector3 direction = (targetPoint.position - transform.position).normalized;
        rb.velocity = direction * speed;

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    void FlipSprite()
    {
        Vector3 moveDirection = transform.position - lastPosition;
        if (moveDirection.x > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (moveDirection.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        lastPosition = transform.position;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
