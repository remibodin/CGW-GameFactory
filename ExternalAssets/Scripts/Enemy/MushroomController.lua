import 'UnityEngine'

local Life = 1.0;
local AttackPower = 1.0;

local ChaseRange = 3.0;
local ChaseSpeed = 2.0;
local ChaseMode = false;

local IsSpiderTouched = false;

function Attacked(power)
    Life = Life - power
    if Life <= 0.0 then
        AudioManager:Play("Sounds/CHAMPI_DEGONFLER_06_1")
        Object.Destroy(this.gameObject, 0.8)
    end
end

function OnCollisionWithPlayer()
    player:TakeDamage(AttackPower)
    AudioManager:Play("Sounds/CHAMPI_POP_B-12_1")
    Object.Destroy(this.gameObject, 0.8)
end

function MushroomIA()
    local distanceToPlayer = Vector3.Distance(player.transform.position, this.transform.position)
    if (not ChaseMode and distanceToPlayer < ChaseRange) then
        ChaseMode = true
    end
    
    if (ChaseMode) then
        local directionToPlayer = player.transform.position.x - this.transform.position.x
        if (directionToPlayer < 0) then
            directionToPlayer = -1.0
        else
            directionToPlayer = 1.0
        end
        local speed = ChaseSpeed
        if (IsSpiderTouched) then
            speed = ChaseSpeed * aragna.TouchSpeedMultiplier
        end
        this:Move(directionToPlayer, speed)
    end
end

function OnCollisionWithDanger()
    Object.Destroy(this.gameObject)
end

function OnCollisionWithSpider()
    IsSpiderTouched = true
    this.SpiderTouchTimer = aragna.TouchTime
end

function Update()
    if (this.SpiderTouchTimer == 0.0) then
        IsSpiderTouched = false
    end
    MushroomIA()
end
