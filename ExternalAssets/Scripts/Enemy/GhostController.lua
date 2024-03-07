import 'UnityEngine'

local Life = 3.0;
local AttackPower = 1.0;
local AttackTimer = 1.0;
local MaxOpacity = 1.0;
local MinOpacity = 0.3;
local MaxOpacityDistance = 3.0;
local MinOpacityDistance = 8.0;

local TookDamage = false;
local KnockbackForce = 1.0;
local KnockbackTimer = 1.0;

function GhostIA()
end

function Start()
    this.Opacity = MinOpacity
end

function Update()
    GhostIA()

    local distanceToPlayer = Vector3.Distance(this.transform.position, player.transform.position)
    if (distanceToPlayer < MinOpacityDistance) then
        if (distanceToPlayer < MaxOpacityDistance) then
            this.Opacity = MaxOpacity
        else
            this.Opacity = Mathf.InverseLerp(MinOpacityDistance, MaxOpacityDistance, distanceToPlayer)
        end
    end
end

function Attacked(power)
    Life = Life - power
    if Life <= 0.0 then
        this:Destroy()
    end
end

function OnCollisionWithPlayer()
    if this.AttackCooldown == 0.0 then
        player:TakeDamage(AttackPower)
        this.AttackCooldown = AttackTimer
    end
end
