using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    [SerializeField] public float MaxHP;
    [SerializeField] public float  CurrentHP { get; private set;}
    [SerializeField] private float _TimeDelay;
    private bool isDead;
    private PlayerController playerController;
    private ComboAttack comboAttack;
    private Animator Anim;
    private void Awake() {
        Anim = GetComponent<Animator>();
        comboAttack = GetComponent<ComboAttack>();
    }
    private void Start() 
    {
        CurrentHP = MaxHP;
    }
    private void TakeDamge(float _Damge)
    {
        if(isDead) return;
        CurrentHP = Mathf.Clamp(CurrentHP - _Damge, 0, MaxHP);
        if(CurrentHP >0)
        {
            StartCoroutine(WaitTime());
            Anim.SetTrigger("Hurt");
            //
        }
        else if(CurrentHP <=0)
        {
            isDead = true;
            Anim.SetTrigger("Dead");
            //gameObject.SetActive(false);
            GetComponent<PlayerController>().enabled = false;
            //comboAttack.isAttacking = false;
        }
    }
    private void Update() 
   {
        if(Input.GetKeyDown(KeyCode.E))
        {
            TakeDamge(MaxHP * 0.1f);
            Debug.Log("10%hp");
        } 
   }    
    private IEnumerator WaitTime()
    {
        GetComponent<PlayerController>().enabled = false;
        yield return new WaitForSeconds(_TimeDelay);
        if(!isDead)
            GetComponent<PlayerController>().enabled = true;

    }
}
