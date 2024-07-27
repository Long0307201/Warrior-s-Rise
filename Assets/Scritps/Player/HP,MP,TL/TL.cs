using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TL : MonoBehaviour
{
    [SerializeField] public float MaxTL;
    [SerializeField] public float  CurrentTL { get; private set;}
    [SerializeField] public float rateTLDown;
    [SerializeField] private float rateTLUp;
    [SerializeField] private float _TimeDelay;
    private bool isRegenerating;
    private PlayerController playerController;
    private void Awake() {
        playerController = GetComponent<PlayerController>();
    }
    private void Start() 
    {
        CurrentTL = MaxTL;
    }
    private void  Update()
    {
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
    private void DecreaseTL()
    {
        CurrentTL -= rateTLDown;
        CurrentTL = Mathf.Clamp(CurrentTL, 0, MaxTL);

        if (CurrentTL <= 0)
        {
            playerController.isSliding = false; // Ngăn không cho chạy nếu MP cạn
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
