import 'UnityEngine'

local CameraSpeed = 3.0;
local CameraAheadDistance = 2.0;
local CameraTurnLimit = 3.0;
local CameraDirection = Vector3.zero;

function Start()
    CameraDirection = player.Facing
    this:InitCameraPosition(player.transform.position, player.Facing, CameraAheadDistance)
end

function Update()
    if CameraDirection.x > 0 then
        if player.transform.position.x < this.transform.position.x - CameraTurnLimit then
            CameraDirection = player.Facing
            this:MoveCamera(CameraAheadDistance * CameraDirection.x, CameraSpeed)
        elseif player.transform.position.x > this.transform.position.x - CameraAheadDistance then
            this:MoveCamera(CameraAheadDistance * CameraDirection.x, CameraSpeed)
        end
    end

    if CameraDirection.x < 0 then
        if player.transform.position.x > this.transform.position.x + CameraTurnLimit then
            CameraDirection = player.Facing
            this:MoveCamera(CameraAheadDistance * CameraDirection.x, CameraSpeed)
        elseif player.transform.position.x < this.transform.position.x + CameraAheadDistance then
            this:MoveCamera(CameraAheadDistance * CameraDirection.x, CameraSpeed)
        end
    end
end