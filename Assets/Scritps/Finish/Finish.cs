using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] GameObject finishScreen;
    [SerializeField] private Transform CheckFinish;
    [SerializeField] private LayerMask FinishLayer;
    [SerializeField] private float radius;
    [SerializeField] bool isFinish;
    private void Start()
    {
        finishScreen.SetActive(false);
    }
    private void Update()
    {
        FinishGame();
        CheckFinishGame();
    }
    private void CheckFinishGame()
    {
        isFinish = Physics2D.OverlapCircle(CheckFinish.position, radius, FinishLayer);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(CheckFinish.position, radius);
    }
    private void FinishGame()
    {
        if (isFinish && Input.GetKey(KeyCode.N))
        {
            finishScreen.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
