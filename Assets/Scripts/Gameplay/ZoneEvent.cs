using Cgw.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneEvent : MonoBehaviour
{
    public bool OneTime = true;
    public EventAction EventAction;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (EventAction != null)
        {
            EventAction.OnActivate();
        }

        if (OneTime)
        {
            gameObject.SetActive(false);
        }
    }
}
