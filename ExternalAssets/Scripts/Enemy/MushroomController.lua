import 'UnityEngine'

local Life = 1.0;
local AttackPower = 1.0;

local ChaseRange = 3.0;
local ChaseSpeed = 1.0;
local ChaseMode = false;

local IsSpiderTouched = false;
local NoMove = false;

function Die()
    -- spawn un fx ici serait mon plus grand rêve
    Object.Destroy(this.gameObject)
end

function Knockback(directionFromPlayer)
    this:AddForceImpulse((Vector3.up + directionFromPlayer) * 3.0)
end

function Attacked(power)
    Life = Life - power
    NoMove = true
    local directionFromPlayer = (this.transform.position - player.transform.position).normalized
    this:DelayAction(0.32, "Knockback", directionFromPlayer)
    if Life <= 0.0 then
        -- AudioManager:Play("Sounds/CHAMPI_DEGONFLER_06_1")
        this:DelayAction(0.7, "Die")
    end
end

function OnCollisionWithPlayer()
    NoMove = true
    player:TakeDamage(AttackPower, this)
    -- AudioManager:Play("Sounds/CHAMPI_POP_B-12_1")
    Object.Destroy(this.gameObject, 0.8)
end

function MushroomIA()
    if (NoMove) then
        return
    end

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
            speed = speed * aragna.TouchSpeedMultiplier
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
