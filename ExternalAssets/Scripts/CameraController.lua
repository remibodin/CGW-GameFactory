import 'UnityEngine'

local CameraSpeed = 3.0;
local CameraAheadDistance = 2.0;
local CameraTurnLimit = 3.0;

function LateUpdate()
    local direction = player.Facing.x;

    local cameraPosition = player.transform.position
    cameraPosition.z = camera.transform.position.z
    cameraPosition.y = player.transform.position.y + 1
    cameraPosition.x = cameraPosition.x + 3 * direction
    camera.transform.position = Vector3.Lerp(camera.transform.position, cameraPosition, Time.deltaTime * 2)
end