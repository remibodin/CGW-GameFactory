using FMODUnity;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Cgw.Gameplay
{
    public class CameraController : SingleBehaviourInScene<CameraController>
    {
        private Camera _camera;
        private UniversalAdditionalCameraData _cameraData;

        protected override void Awake()
        {
            base.Awake();

            _camera = GetComponent<Camera>();
            _cameraData = GetComponent<UniversalAdditionalCameraData>();
        }

        private void OnEnable()
        {
            RuntimeManager.StudioSystem.setNumListeners(1);
        }

        private void OnDisable()
        {
            RuntimeManager.StudioSystem.setNumListeners(0);
        }

        private void Update()
        {
            GameObject targetEffect = gameObject;

            if (Player.Instance != null)
            {
                targetEffect = Player.Instance.gameObject;
            }

            RuntimeManager.SetListenerLocation(targetEffect);
            _cameraData.volumeTrigger = targetEffect.transform;
        }

        private void LateUpdate()
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
            cameraPosition.y = 0.0f;
            transform.position = Vector3.Lerp(transform.position, cameraPosition, Time.deltaTime * 2);
        }
    }
}