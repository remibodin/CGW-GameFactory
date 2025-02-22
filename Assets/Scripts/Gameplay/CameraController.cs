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
            // FMOD aime pas le set a 0
            // RuntimeManager.StudioSystem.setNumListeners(0); 
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

            if (Input.GetKeyDown(KeyCode.F1))
            {
                _camera.orthographicSize = 1.1f;
            }
            if (Input.GetKeyDown(KeyCode.F2))
            {
                _camera.orthographicSize = 1.3f;
            }
            if (Input.GetKeyDown(KeyCode.F3))
            {
                _camera.orthographicSize = 1.5f;
            }
            if (Input.GetKeyDown(KeyCode.F4))
            {
                _camera.orthographicSize = 1.7f;
            }
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