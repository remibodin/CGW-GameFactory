import 'UnityEngine'

local Life = 3.0;
local LifePerLaunch = 0.5;

local FollowSpeed = 2.0;
local FollowDistance = 0.1;
local FollowHeight = 0.6;

local CanLaunch = true;
local LaunchRange = 6.0;
local LaunchSpeed = 4.0;
local TouchTime = 3.0;
local TouchSpeedMultiplier = 0.1;

local HasTarget = false;
local Target = nil;
local TargetPoint = Vector3.zero;

function Start()
    this.TouchTime = TouchTime
    this.TouchSpeedMultiplier = TouchSpeedMultiplier

    if (player ~= nil) then
        local targetPosition = player.transform.position + Vector3.up * FollowHeight
        if (player.Facing.x > 0.0) then
            targetPosition = targetPosition + Vector3.left * FollowDistance
        else
            targetPosition = targetPosition + Vector3.right * FollowDistance
        end
    end
    this.transform.position = targetPosition
end

function Launch()
    if (CanLaunch and Life > 0) then
        local enemy = this:CheckLaunch(player.transform.position + Vector3.up * FollowHeight, player.Facing, LaunchRange)
        if not enemy == nil then
            HasTarget = true
            Target = enemy
            Life = Life - LifePerLaunch
        else
            HasTarget = true
            Target = nil
            TargetPoint = player.transform.position + player.Facing * LaunchRange
        end
    end
end

function OnCollisionWithEnemy()
    HasTarget = false
end

function Update()
    local targetPosition = player.transform.position + Vector3.up * FollowHeight
    if not HasTarget and Vector3.Distance(this.transform.position, targetPosition) > FollowDistance then
        if this.transform.position.x < targetPosition.x then
            this:MoveFollow(targetPosition + Vector3.left * FollowDistance, FollowSpeed)
        else
            this:MoveFollow(targetPosition + Vector3.right * FollowDistance, FollowSpeed)
        end
    elseif HasTarget then
        if (not Target == nil) then
            this:MoveTowards(Target.transform.position, LaunchSpeed)
        else
            this:MoveTowards(TargetPoint, LaunchSpeed)
            if Vector3.Distance(TargetPoint, this.transform.position) < 0.1 then
                HasTarget = false
            end
        end
    end
end