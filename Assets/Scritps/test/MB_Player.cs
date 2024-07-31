using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MB_Player : MonoBehaviour
{
    public float currentSpeed = 5f;
    private Rigidbody2D rb;
    private float horizontalInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = new Vector2(horizontalInput * currentSpeed, rb.velocity.y);
    }

    public void MoveLeft()
    {
        horizontalInput = -1f;
    }

    public void MoveRight()
    {
        horizontalInput = 1f;
    }

    public void StopMoving()
    {
        horizontalInput = 0f;
    }
}
