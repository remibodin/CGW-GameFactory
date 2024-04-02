import 'UnityEngine'

local CameraSpeed = 3.0;
local CameraAheadDistance = 0.5;
local CameraTurnLimit = 1.0;

function LateUpdate()
    local direction = player.Facing.x;

    local cameraPosition = player.transform.position
    cameraPosition.z = camera.transform.position.z
    cameraPosition.x = cameraPosition.x + 1.2 * direction
    cameraPosition.y = 0.0
    camera.transform.position = Vector3.Lerp(camera.transform.position, cameraPosition, Time.deltaTime * 2)
end