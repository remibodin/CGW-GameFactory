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
            local horizontalAxis = Input.GetAxis("Horizontal")
            if (Mathf.Abs(horizontalAxis) > 0.0) then
                if (this.JumpCooldown == 0.0 and this.OnMaterial ~= "Slope") then
                    this:Move(Speed, Input.GetAxis("Horizontal"))
                end
            end

            local jumpAxis = Input.GetAxis("Jump")
            if (jumpAxis > 0 and (this.OnGround or this.OnMaterial == "Slope")) then
                if (this.JumpCooldown == 0.0) then
                    Inertia = this.Motion.x
                    this:Jump(JumpForce * jumpAxis)
                    this.JumpCooldown = JumpTime
                end
            end
        else
            local horizontalAxis = Input.GetAxis("Horizontal")
            this:MoveWithInertia(AirSpeed, horizontalAxis, Inertia)
        end

        if (Input.GetKeyDown("f")) then
            if (this.AttackCooldown == 0.0) then
                this:Attack(AttackRange, AttackPower)
                this.AttackCooldown = AttackTime
            end
        end

        if (Input.GetKeyDown("g")) then
            if (this.LaunchCooldown == 0.0) then
                aragna:Launch()
                this.LaunchCooldown = LaunchTimer
            end
        end
    end

    PreviousOnGround = this.OnGround
    PreviousOnMaterial = this.OnMaterial
end


function OnAnimEvent(animEvent)
    if (animEvent == "AttackSound") then
        AudioManager:Play("Sounds/HERO_ATTAQUE_WHOOSH-05_1")
    elseif (animEvent == "HeroLanding") then
        AudioManager:PlayRandom("Sounds/Collections/JumpDirt")
    elseif (animEvent == "HeroJumpingStart") then
        -- AudioManager:PlayRandom("Sounds/Collections/JumpDirt")
end
