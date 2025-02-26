using FMODUnity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Cgw.Gameplay
{
    public class CameraController : SingleBehaviourInScene<CameraController>
    {
        [Serializable]
        public class CameraRail
        {
            public float MaxCharacterY;
            public float MinCharacterY;
            public float CameraY;
        }

        public List<CameraRail> CameraRails;

        private Camera _camera;
        private UniversalAdditionalCameraData _cameraData;
        private CameraRail m_ActiveCameraRail;

        protected override void Awake()
        {
            base.Awake();

            _camera = GetComponent<Camera>();
            _cameraData = GetComponent<UniversalAdditionalCameraData>();
            _camera.orthographicSize = 1.3f;
        }

        public void OnEnable()
        {
            RuntimeManager.StudioSystem.setNumListeners(1);
        }

        public void OnDisable()
        {
            // FMOD aime pas le set a 0
            // RuntimeManager.StudioSystem.setNumListeners(0); 
        }

        public void Update()
        {
            GameObject targetEffect = gameObject;

            if (Player.Instance != null)
            {
                targetEffect = Player.Instance.gameObject;
            }

            RuntimeManager.SetListenerLocation(targetEffect);
            _cameraData.volumeTrigger = targetEffect.transform;

            if (CameraRails.Count == 0 )
            {
                m_ActiveCameraRail = null;
            }
            else
            {
                var actualCameraRail = CameraRails.FirstOrDefault(SelectCameraRail);
                if (actualCameraRail != null)
                {
                    m_ActiveCameraRail = actualCameraRail;
                }
            }
        }

        private bool SelectCameraRail(CameraRail rail)
        {
            if (Player.Instance != null)
            {
                Vector3 playerPos = Player.Instance.transform.position;
                if (rail.MaxCharacterY >= playerPos.y && rail.MinCharacterY <= playerPos.y)
                {
                    return true;
                }
            }
            return false;
        }

        public void LateUpdate()
        {
            float direction = 0.0f;
            Vector3 cameraPosition = PlayerSpawner.Instance.transform.position;
            Player player = Player.Instance;

            if (player != null)
            {
                direction = player.Facing.x;
                cameraPosition = player.transform.position;
            }

            cameraPosition.z = transform.position.z;
            cameraPosition.x = cameraPosition.x + 1.2f * direction;

            if (m_ActiveCameraRail != null)
            {
                cameraPosition.y = m_ActiveCameraRail.CameraY;
            }
            else
            {
                cameraPosition.y = 0.0f;
            }

            transform.position = Vector3.Lerp(transform.position, cameraPosition, Time.deltaTime * 2);
        }
    }
}