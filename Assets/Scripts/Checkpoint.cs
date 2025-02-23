using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    public UnityEvent OnActivated;

    private bool _activated = false;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.gameObject.CompareTag("Player")
            || _activated)
        {
            return;
        }

        _activated = true;
        PlayerSpawner.Instance.transform.position = transform.position;
        OnActivated.Invoke();
    }
}
