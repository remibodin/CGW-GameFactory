using UnityEngine;

public class PhysicTriggerSetAnimatorTrigger : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _triggerName;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            _animator.SetTrigger(_triggerName);
        }
    }
}
