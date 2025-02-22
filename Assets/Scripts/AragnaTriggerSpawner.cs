using Cgw.Gameplay;
using UnityEngine;

public class AragnaTriggerSpawner : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") &&
            SpiderController.Instance == null)
        {
            PlayerSpawner.Instance.SpawnAragna();
        }
    }
}
