using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboAttack : MonoBehaviour
{
    [Header("ComboGround")]    
    [SerializeField] private int NumberCombo;
    [SerializeField] private float TimeCombo;
    [SerializeField] private float DistanceCombo;
    [SerializeField] private float ComboTempo;
    private int Combo;
    public bool isAttacking;
    public bool isAirAttacking;
    [Header("Attack")]
    [SerializeField] private float TakeDamgeEnemies;
    [SerializeField] private Transform AttackPoint;
    [SerializeField] private LayerMask Enemies;
    [SerializeField] private float radius;
    private PlayerController playerController;
    private HP hP;
    private Animator Anim;
    private Rigidbody2D rb;
    private void Awake() 
    {
        Anim = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        hP = GetComponent<HP>();
    }
    private void Start() 
    {
        ComboTempo = TimeCombo;
    }
    private void Update() 
    {
        if(hP.CurrentHP > 0 )
        {
            if (playerController.isGrounded)
            {
                DoCombo();
            }
            else 
            {
               // playerController.canJump = false;
                DoAirCombo();
            }
            //DoCombo();
            // DoAirCombo();
        }
    }
    private void DoCombo()
    {
        ComboTempo -=Time.deltaTime;
        if(Input.GetKey(KeyCode.L) && ComboTempo <0)
        {
            isAttacking = true;
            Anim.SetTrigger("A"+ Combo);
            ComboTempo = TimeCombo;
            StartCoroutine(WaitCombo());
        }
        else if(Input.GetKey(KeyCode.L) && ComboTempo >0 && ComboTempo < DistanceCombo)
        {
            isAttacking = true;
            Combo ++;
            if(Combo>NumberCombo)
            {
                Combo =1;
            }
            Anim.SetTrigger("A" + Combo);
            ComboTempo = TimeCombo;
            StartCoroutine(WaitCombo());
        }
        else if(!Input.GetKey(KeyCode.L) && ComboTempo < 0 )
        {
            isAttacking = false;
        }
        if(ComboTempo < 0)
        {
            Combo = 1;
        }
    }
    public void DoAirCombo()
    {
        ComboTempo -=Time.deltaTime;
        if(Input.GetKey(KeyCode.L) && ComboTempo <0)
        {
            isAirAttacking = true;
            Anim.SetTrigger("EA"+ Combo);
            ComboTempo = TimeCombo;
            StartCoroutine(WaitCombo());
        }
        else if(Input.GetKey(KeyCode.L) && ComboTempo >0 && ComboTempo < DistanceCombo)
        {
            isAirAttacking = true;
            Combo ++;
            if(Combo>NumberCombo)
            {
                Combo =1;
            }
            Anim.SetTrigger("EA" + Combo);
            ComboTempo = TimeCombo;
            StartCoroutine(WaitCombo());
        }
        else if(!Input.GetKey(KeyCode.L) && ComboTempo < 0 )
        {
            isAirAttacking = false;
        }
        if(ComboTempo < 0)
        {
            Combo = 1;
        }
    }
    private IEnumerator WaitCombo()
    {
        GetComponent<PlayerController>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        GetComponent<PlayerController>().enabled = true;

    }
    public void Attack()
    {
        Collider2D [] Enemy = Physics2D.OverlapCircleAll(AttackPoint.position,radius,Enemies);
        foreach(Collider2D gameobjectEnemy in Enemy)
        {
            gameobjectEnemy.GetComponent<HPEnemies>().TakeDameEnemies(TakeDamgeEnemies);
        }
    }
    private void  OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(AttackPoint.position,radius);
    }

}
