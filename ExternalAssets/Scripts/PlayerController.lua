import 'UnityEngine'

local Life = 3;
local Speed = 2.5;
local AirSpeed = 2.5;
local JumpTime = 0.3;
local JumpForce = 9.0;
local LaunchTimer = 6.0;

local AttackTime = 1.0;
local DamageTime = 0.8;
local AttackRange = 2.5;
local AttackPower = 1.0;

local SinceLastFootStep = 0;

function TakeDamage(power)
    if this.DamageCooldown == 0.0 then
        Life = Life - power
        this.DamageCooldown = DamageTime
        if Life <= 0.0 then
            Object.Destroy(this.gameObject)
        end
    end
end

function Update()
    local jumpAxis = Input.GetAxis("Jump")
    if (jumpAxis > 0 and (this.OnGround or this.OnMaterial == "Slope")) then
        if (this.JumpCooldown == 0.0) then
            this:Jump(JumpForce * jumpAxis)
            this.JumpCooldown = JumpTime
        end
    end

    local horizontalAxis = Input.GetAxis("Horizontal")
    if (Mathf.Abs(horizontalAxis) > 0.0) then
        if (this.OnGround and this.JumpCooldown == 0.0 and this.OnMaterial ~= "Slope") then
            this:Move(Speed, Input.GetAxis("Horizontal"))
        end

        if (not this.OnGround) then
            this:Move(AirSpeed, Input.GetAxis("Horizontal"))
        end
    end

    if (Input.GetKeyDown("f")) then
        if (this.AttackCooldown == 0.0) then
            this:Attack(AttackRange, AttackPower)
            AudioManager:Play("Sounds/HERO_ATTAQUE_WHOOSH-05_1")
            this.AttackCooldown = AttackTime
        end
    end

    if (Input.GetKeyDown("g")) then
        if (this.LaunchCooldown == 0.0) then
            aragna:Launch()
            this.LaunchCooldown = LaunchTimer
        end
    end

    if (this.Motion.magnitude > 0 and this.OnGround) then
        SinceLastFootStep = SinceLastFootStep + Time.deltaTime
        if (SinceLastFootStep > 0.32) then
            if (this.OnMaterial == "Dirt") then
                AudioManager:PlayRandom('Sounds/Collections/FootStepsDirtRight')
            elseif (this.OnMaterial == "Wood") then

            end
            SinceLastFootStep = 0;
        end
    end
end
