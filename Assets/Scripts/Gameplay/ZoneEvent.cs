using Cgw.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneEvent : MonoBehaviour
{
    public bool Activated = false;
    public EventAction EventAction;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (EventAction != null && !Activated)
        {
            EventAction.OnActivate();
            Activated = true;
        }
    }

    private void Update()
    {
        if (Activated)
        {
            if (EventAction != null)
            {
                EventAction.OnUpdate();
            }
        }
    }
}
