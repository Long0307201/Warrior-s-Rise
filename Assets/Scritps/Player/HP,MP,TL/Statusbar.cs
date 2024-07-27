using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statusbar : MonoBehaviour
{
    [Header("HP")]
    [SerializeField] private Image MaxHPBar;
    [SerializeField] private Image CurrentHPBar;
    [SerializeField] private HP hp;
    [Header("MP")]
    [SerializeField] private Image MaxMPBar;
    [SerializeField] private Image CurrentMPBar;
    [SerializeField]private MP mp;
    [Header("TL")]
    [SerializeField] private Image MaxTLBar;
    [SerializeField] private Image CurrentTLBar;
    [SerializeField] private TL tL;
    private void Start() 
    {
        CurrentHPBar.fillAmount = 1.0f;
        CurrentMPBar.fillAmount = 1.0f;
        CurrentTLBar.fillAmount = 1.0f;
    }
    private void Update()
    {
        HPBar();
        MPBar();
        TLBar();
    }
    private void HPBar()
    {
        CurrentHPBar.fillAmount = hp.CurrentHP/hp.MaxHP;
    }
    private void MPBar()
    {
        CurrentMPBar.fillAmount = mp.CurrentMP/mp.MaxMP;
    }
    private void TLBar()
    {
        CurrentTLBar.fillAmount = tL.CurrentTL/tL.MaxTL;
    }
}
