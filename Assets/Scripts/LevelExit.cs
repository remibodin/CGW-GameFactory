using UnityEngine;

using Cgw.Assets;

public class LevelExit : MonoBehaviour
{
    [SerializeField] private string _nextSceneName;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            SceneLoader.Custom(_nextSceneName);
        }
    }
}
