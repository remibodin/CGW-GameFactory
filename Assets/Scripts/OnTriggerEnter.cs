using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEnter : MonoBehaviour
{
    public UnityEvent onTriggerEnter;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            onTriggerEnter.Invoke();
        }
    }
}
