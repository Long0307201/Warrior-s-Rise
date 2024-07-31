using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomentButton : MonoBehaviour
{
    private ComboAttack comboAttack;
    void Awake()
    {
        comboAttack = GetComponent<ComboAttack>();
    }
    public void attack()
    {
    }
}
