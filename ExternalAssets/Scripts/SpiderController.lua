import 'UnityEngine'

local Life = 3.0;
local LifePerLaunch = 0.5;
local FollowSpeed = 2.0;
local FollowDistance = 0.5;
local FollowHeight = 0.3

function Start()
    local targetPosition = player.transform.position + Vector3.up * FollowHeight
    if (player.Facing.x > 0.0) then
        targetPosition = targetPosition + Vector3.left * FollowDistance
    else
        targetPosition = targetPosition + Vector3.right * FollowDistance
    end
    this.transform.position = targetPosition
end

function Launch()
    if (Life > 0) then
        Life = Life - LifePerLaunch
    end
end

function Update()
    local targetPosition = player.transform.position + Vector3.up * FollowHeight
    if Vector3.Distance(this.transform.position, targetPosition) > FollowDistance then
        if this.transform.position.x < targetPosition.x then
            this:MoveFollow(targetPosition + Vector3.left * FollowDistance, FollowSpeed)
        else
            this:MoveFollow(targetPosition + Vector3.right * FollowDistance, FollowSpeed)
        end
    end
end