import 'UnityEngine'

local Life = 1.0;
local AttackPower = 1.0;

local ChaseRange = 3.0;
local ChaseSpeed = 2.0;
local ChaseMode = false;

function Attacked(power)
    Life = Life - power
    if Life <= 0.0 then
        Object.Destroy(this.gameObject)
    end
end

function OnCollisionWithPlayer()
    player:TakeDamage(AttackPower)
    Object.Destroy(this.gameObject)
end

function MushroomIA()
    local distanceToPlayer = Vector3.Distance(player.transform.position, this.transform.position)
    if (not ChaseMode and distanceToPlayer < ChaseRange) then
        ChaseMode = true
    elseif (ChaseMode) then
        local directionToPlayer = player.transform.position.x - this.transform.position.x
        if (directionToPlayer < 0) then
            directionToPlayer = -1.0
        else
            directionToPlayer = 1.0
        end
        this:Move(directionToPlayer, ChaseSpeed)
    end
end

function OnCollisionWithDanger()
    Object.Destroy(this.gameObject)
end

function Update()
    MushroomIA()
end
