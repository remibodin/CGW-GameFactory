import 'UnityEngine'

local Life = 3;
local Speed = 1.5;
local AirSpeed = 2.5;
local JumpTime = 0.85;
local JumpForce = 8.0;

local AttackTime = 1.0;
local DamageTime = 0.8;
local AttackRange = 1.5;
local AttackPower = 1.0;

local SinceLastFootStep = 0;

function TakeDamage(power)
    if this.DamageCooldown == 0.0 then
        Life = Life - power
        this.DamageCooldown = DamageTime
        if Life <= 0.0 then
            this:Destroy()
        end
    end
end

function Update()
    local jumpAxis = Input.GetAxis("Jump")
    if (jumpAxis > 0 and this.OnGround) then
        if (this.JumpCooldown == 0.0) then
            this:Jump(JumpForce * jumpAxis)
            this.JumpCooldown = JumpTime
        end
    end

    if (this.OnGround and this.JumpCooldown == 0.0 and this.OnMaterial ~= "Slope") then
        this:Move(Speed, Input.GetAxis("Horizontal"))
    end

    if (not this.OnGround) then
        this:Move(AirSpeed, Input.GetAxis("Horizontal"))
    end

    if (Input.GetKeyDown("f")) then
        if (this.AttackCooldown == 0.0) then
            this:Attack(AttackRange, AttackPower)
            this.AttackCooldown = AttackTime
        end
    end

    if (this.Motion.magnitude > 0 and this.OnGround) then
        SinceLastFootStep = SinceLastFootStep + Time.deltaTime
        if (SinceLastFootStep > 0.32) then
            AudioManager:PlayRandom('Sounds/Collections/FootStep')
            SinceLastFootStep = 0;
        end
    end
end