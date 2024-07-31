using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statusbar : MonoBehaviour
{
    [Header("HP")]
    [SerializeField] private Image MaxHPBar;
    [SerializeField] private Image CurrentHPBar;

    [Header("MP")]
    [SerializeField] private Image MaxMPBar;
    [SerializeField] private Image CurrentMPBar;

    [Header("TL")]
    [SerializeField] private Image MaxTLBar;
    [SerializeField] private Image CurrentTLBar;

    [SerializeField] private PlayerStats playerStats;
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
        CurrentHPBar.fillAmount = playerStats.CurrentHP / playerStats.MaxHP;
    }
    private void MPBar()
    {
        CurrentMPBar.fillAmount = playerStats.CurrentMP / playerStats.MaxMP;
    }
    private void TLBar()
    {
        CurrentTLBar.fillAmount = playerStats.CurrentTL / playerStats.MaxTL;
    }
}
