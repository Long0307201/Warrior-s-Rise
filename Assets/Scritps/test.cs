using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public float slideSpeed = 10f;
    public float slideDuration = 0.5f;

    private Rigidbody2D rb;
    private bool isSliding = false;
    private float slideTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isSliding)
        {
            StartSlide();
        }

        if (isSliding)
        {
            slideTimer += Time.deltaTime;
            if (slideTimer >= slideDuration)
            {
                StopSlide();
            }
        }
    }

    void StartSlide()
    {
        isSliding = true;
        slideTimer = 0f;
        rb.velocity = new Vector2(transform.localScale.x * slideSpeed, rb.velocity.y);
    }

    void StopSlide()
    {
        isSliding = false;
        rb.velocity = new Vector2(0, rb.velocity.y);
    }
}
