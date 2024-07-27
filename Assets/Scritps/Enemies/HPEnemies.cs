using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPEnemies : MonoBehaviour
{
    [SerializeField] private float MaxHPEnemies;
    [SerializeField] public float CurrentHPEnemies { get ; private set;}
    [SerializeField] private float _TimeDelay;
    [SerializeField] private Transform hitParticle;
    private bool isDead;
    [SerializeField]  Animator Anim;
    private void Awake() {
        Anim = GetComponent<Animator>();
    }
    private void Start()
    {
        CurrentHPEnemies = MaxHPEnemies;
    }
    public void TakeDameEnemies(float _TakeDamgeEnemies)
    {
        if(isDead) return;
        CurrentHPEnemies = Mathf.Clamp(CurrentHPEnemies - _TakeDamgeEnemies,0,MaxHPEnemies);
        if(CurrentHPEnemies > 0)
        {
            Anim.SetTrigger("Hurt");
            Instantiate(hitParticle, Anim.transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
        }
        else if(CurrentHPEnemies <=0)
        {
            isDead = true;
            gameObject.SetActive(false);
        }
    }
}
