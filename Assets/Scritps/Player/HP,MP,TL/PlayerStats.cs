using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("HP")]
    [SerializeField] public float MaxHP;
    [SerializeField] public float CurrentHP { get; private set; }
    [SerializeField] private float _TimeDelay;
    [SerializeField] GameObject lostScreen;
    private bool isDead;
    private Animator anim;
    private PlayerController playerController;

    [Header("MP")]
    [SerializeField] public float MaxMP;
    [SerializeField] public float CurrentMP { get; private set; }

    [Header("TL")]
    [SerializeField] public float MaxTL;
    [SerializeField] public float CurrentTL { get; private set; }
    [SerializeField] public float rateTLDown;
    [SerializeField] private float rateTLUp;
    private bool isRegenerating;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        CurrentHP = MaxHP;
        CurrentMP = MaxMP;
        CurrentTL = MaxTL;
        lostScreen.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage(MaxHP * 0.1f);
            Debug.Log("10% HP");
        }

        if (playerController.isSliding)
        {
            DecreaseTL();
            StopCoroutine(RegenerateTL());
            isRegenerating = false;
        }
        else if (!isRegenerating)
        {
            StartCoroutine(RegenerateTL());
        }
    }
    //HP
    private void TakeDamage(float damage)
    {
        if (isDead) return;
        CurrentHP = Mathf.Clamp(CurrentHP - damage, 0, MaxHP);
        if (CurrentHP > 0)
        {
            StartCoroutine(WaitTime());
            anim.SetTrigger("Hurt");
        }
        else if (CurrentHP <= 0)
        {
            isDead = true;
            anim.SetTrigger("Dead");
            playerController.enabled = false;
            lostScreen.SetActive(true);
        }
    }

    private IEnumerator WaitTime()
    {
        playerController.enabled = false;
        yield return new WaitForSeconds(_TimeDelay);
        if (!isDead)
            playerController.enabled = true;
    }
    //TL
    private void DecreaseTL()
    {
        CurrentTL -= rateTLDown;
        CurrentTL = Mathf.Clamp(CurrentTL, 0, MaxTL);

        if (CurrentTL <= 0)
        {
            playerController.isSliding = false; // Ngăn không cho trượt nếu TL cạn
        }
    }

    private IEnumerator RegenerateTL()
    {
        isRegenerating = true;
        yield return new WaitForSeconds(_TimeDelay);

        while (CurrentTL < MaxTL && !playerController.isSliding)
        {
            CurrentTL += rateTLUp * Time.deltaTime;
            CurrentTL = Mathf.Clamp(CurrentTL, 0, MaxTL);
            yield return null;
        }
        isRegenerating = false;
    }
}
