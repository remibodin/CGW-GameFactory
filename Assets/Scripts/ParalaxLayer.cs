using UnityEngine;

namespace Cgw
{
    public class ParalaxLayer : MonoBehaviour
    {
        [Range(-1, 1)]
        public float ParalaxRatio;
        private Vector3 m_initialPosition;
        private Vector3 m_initialCameraPosition;
        private Camera m_camera;

        private void Start()
        {
            m_camera = Camera.main;
            m_initialPosition = transform.position;
            m_initialCameraPosition = m_camera.transform.position;
        }

        private void Update()
        {
            var position = m_camera.transform.position;
            position.x = m_camera.transform.position.x * ParalaxRatio;
            position.y = 0;
            position.z = 0;
            transform.position = m_initialPosition + position;
        }
    }
}