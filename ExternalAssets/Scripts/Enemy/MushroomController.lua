import 'UnityEngine'

local Life = 1.0;
local AttackPower = 1.0;
local AttackTimer = 1.0;

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