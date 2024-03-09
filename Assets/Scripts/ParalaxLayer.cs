using UnityEngine;

namespace Cgw
{
    public class ParalaxLayer : MonoBehaviour
    {
        [Range(-10, 10)]
        public float ParalaxRatio;
        private Vector3 m_initialPosition;
        private Camera m_camera;

        private void Start()
        {
            m_camera = Camera.main;
            m_initialPosition = transform.position;
        }

        private void Update()
        {
            var position = m_camera.transform.position;
            position.x = m_camera.transform.position.x * ParalaxRatio;
            position.y = m_initialPosition.y;
            position.z = 0;
            transform.position = m_initialPosition + position;
        }
    }
}