using UnityEngine;
using UnityEngine.UI;

namespace Cgw.Gameplay
{
    public class BlobShadow : MonoBehaviour
    {
        public LayerMask layerMask;
        [SerializeField] private SpriteRenderer _graphic;

        private void LateUpdate()
        {
            var hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, layerMask);
            if (!hit)
            {
                _graphic.enabled = false;
                return;
            }
            _graphic.enabled = true;
            _graphic.transform.position = hit.point;
        }
    }
}
