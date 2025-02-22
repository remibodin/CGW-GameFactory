using UnityEngine;

namespace Cgw.Gameplay
{
    public class CameraController : SingleBehaviour<CameraController>
    {
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