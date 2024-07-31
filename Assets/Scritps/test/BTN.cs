using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BTN : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public MB_Player mB_Player;
    public bool isLeftButton;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isLeftButton)
        {
            mB_Player.MoveLeft();
        }
        else
        {
            mB_Player.MoveRight();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        mB_Player.StopMoving();
    }
}
