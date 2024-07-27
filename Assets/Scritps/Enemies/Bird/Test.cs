using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
[SerializeField] private Transform LeftEdge;
[SerializeField] private Transform RightEdge;
[SerializeField] private float Speed;
private bool movingLeft;
private Animator Anim;

private void Awake() {
    Anim = GetComponent<Animator>();
}

private void Update() 
{
    Move();
}

private void Move()
{
    // Kiểm tra hướng di chuyển
    if (movingLeft)
    {
        if (transform.position.x > LeftEdge.position.x)
            MoveInDirection(-1);
        else
            movingLeft = false;
    }
    else
    {
        if (transform.position.x < RightEdge.position.x)
            MoveInDirection(1);
        else
            movingLeft = true;
    }
}

private void MoveInDirection(int _direction) //_direction hướng di chuyển
{
    transform.position = new Vector2(transform.position.x + Time.deltaTime * _direction * Speed, transform.position.y);
    // Xoay mặt nhân vật theo hướng di chuyển
    transform.localScale = new Vector3(_direction, 1, 1);
}
}
