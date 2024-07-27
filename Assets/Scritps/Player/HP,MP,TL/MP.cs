using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP : MonoBehaviour
{
    [SerializeField] public float MaxMP;
    [SerializeField] public float  CurrentMP { get; private set;}
    private void Start() 
    {
        CurrentMP = MaxMP;
    }
}
