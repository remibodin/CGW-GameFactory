import 'UnityEngine'

local Life = 3;
local Speed = 0.65;
local AirSpeed = 1.7;
local JumpTime = .85;
local JumpForce = 7;
local LaunchTimer = 6.0;

local AttackTime = 0.6;
local DamageTime = 0.8;
local AttackRange = 0.8;
local AttackPower = 1.0;

local SinceLastFootStep = 0;

local PreviousXPosition = 0.0
local PreviousOnMaterial = "Dirt"
local PreviousOnGround = true
local NoControl = false
local Inertia = 0.0

local KillY = -1000.0

local HorizontalInputAction = nil
local JumpInputAction = nil
local AttackInputAction = nil
local InteractInputAction = nil
local AragnaAttackInputAction = nil

function TookDamage()
    if Life <= 0.0 then
        Object.Destroy(this.gameObject)
    end
    NoControl = false
end

function TakeDamage(power, enemy)
    if this.DamageCooldown == 0.0 then
        NoControl = true
        Life = Life - power
        this.DamageCooldown = DamageTime
        local directionFromEnemy = (this.transform.position - enemy.transform.position).normalized
        this:AddForceImpulse((Vector3.up + directionFromEnemy) * 3.0)
        this:DelayAction(0.5, "TookDamage")
    end
end

function Start()
    PreviousOnGround = this.OnGround
    PreviousOnMaterial = this.OnMaterial
end

function Update()
    if (this.OnGround and not PreviousOnGround) then
        Inertia = 0.0
    end

    if (not NoControl) then
        if (this.OnMaterial == "Slope" and PreviousOnMaterial ~= "Slope") then
            this.JumpCooldown = 0.0
        end

        if (this.OnGround) then
            local horizontalAxis = this:GetHorizontalInput()
            if (Mathf.Abs(horizontalAxis) > 0.0) then
                if (this.JumpCooldown == 0.0 and this.OnMaterial ~= "Slope") then
                    this:Move(Speed, horizontalAxis)
                end
            end
        else
            local horizontalAxis = this:GetHorizontalInput()
            this:MoveWithInertia(AirSpeed, horizontalAxis, Inertia)
        end
    end

    PreviousOnGround = this.OnGround
    PreviousOnMaterial = this.OnMaterial

    if this.transform.position.y <= KillY then
        Object.Destroy(this.gameObject);
    end
end

function OnAragnaAttack()
    if (this.LaunchCooldown == 0.0) then
        aragna:Launch()
        this.LaunchCooldown = LaunchTimer
    end
end

function OnAttack()
    if (this.AttackCooldown == 0.0) then
        this:Attack(AttackRange, AttackPower)
        this.AttackCooldown = AttackTime
    end
end

function OnJump()
    if this.OnGround or this.OnMaterial == "Slope" then
        if (this.JumpCooldown == 0.0) then
            Inertia = this.Motion.x
            this:Jump(JumpForce)
            this.JumpCooldown = JumpTime
        end
    end
end

function PlayFootStep()
    AudioManager:PlayFmodEvent('event:/Player/FootStep')
    -- if (this.OnMaterial == "Dirt") then
    --     AudioManager:PlayRandom('Sounds/Collections/FootStepsDirtRight')
    -- elseif (this.OnMaterial == "Wood") then

    -- end
end

function OnAnimEvent(animEvent)
    if (animEvent == "AttackSound") then
        AudioManager:Play("Sounds/HERO_ATTAQUE_WHOOSH-05_1")
    elseif (animEvent == "HeroLanding") then
        AudioManager:PlayRandom("Sounds/Collections/JumpDirt")
    elseif (animEvent == "HeroJumpingStart") then
        -- AudioManager:PlayRandom("Sounds/Collections/JumpDirt")
    elseif (animEvent == "HeroStep") then
        PlayFootStep()
    end
end
