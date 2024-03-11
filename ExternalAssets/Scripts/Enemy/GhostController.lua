import 'UnityEngine'

local Life = 3.0;
local MaxOpacity = 1.0;
local MinOpacity = 0.3;
local MaxOpacityDistance = 3.0;
local MinOpacityDistance = 8.0;

local ChaseDistance = 6.0;
local ChaseSpeed = 2.0;
local ChaseMode = false;
local ChargeMode = false;
local ChargeDistance = 1.0;
local ChargeTime = 3.0;
local ExplosionDamage = 1.0;

local IsSpiderTouched = false;

function UpdateOpacity(distanceToPlayer)
    if (distanceToPlayer < MinOpacityDistance) then
        if (distanceToPlayer < MaxOpacityDistance) then
            this.Opacity = MaxOpacity
        else
            this.Opacity = Mathf.InverseLerp(MinOpacityDistance, MaxOpacityDistance, distanceToPlayer)
        end
    end
end

function OnCollisionWithSpider()
    IsSpiderTouched = true
    this.SpiderTouchTimer = aragna.TouchTime
end

function GhostIA()
    local distanceToPlayer = Vector3.Distance(this.transform.position, player.transform.position + Vector3.up)
    UpdateOpacity(distanceToPlayer)
    if (not ChaseMode and distanceToPlayer < ChaseDistance) then
        ChaseMode = true
    end
    
    if (ChaseMode and not ChargeMode) then
        if (distanceToPlayer > ChargeDistance) then
            ChargeMode = false
            local speed = ChaseSpeed
            if (IsSpiderTouched) then
                speed = ChaseSpeed * aragna.TouchSpeedMultiplier
            end
            this:Move(((player.transform.position + Vector3.up) - this.transform.position).normalized, speed)
        else
            ChargeMode = true
            this.ChargeCountdown = ChargeTime
        end
    end

    if (ChargeMode) then
        if (distanceToPlayer > ChargeDistance) then
            ChargeMode = false
            this.ChargeCountdown = 0.0
        elseif (this.ChargeCountdown == 0.0) then
            player:TakeDamage(ExplosionDamage, this)
            Object.Destroy(this.gameObject)
        end
    end
end

function Start()
    this.Opacity = MinOpacity
end

function Update()
    if (this.SpiderTouchTimer == 0.0) then
        IsSpiderTouched = false
    end
    GhostIA()
end

function Attacked(power)
    Life = Life - power
    if Life <= 0.0 then
        Object.Destroy(this.gameObject, 0.8)
    end
end
