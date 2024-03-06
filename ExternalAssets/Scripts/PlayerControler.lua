import 'UnityEngine'

local Life = 3;
local Speed = 1.0;
local AirSpeed = 0.8;
local AttackRange = 1.5;
local AttackPower = 1.0;
local JumpTime = 0.2;
local AttackTime = 1.0;
local JumpForce = 100.0;

function Update()
    if (Input.GetKeyDown("space") and this.OnGround) then
        if this.JumpCooldown == 0.0 then
            this:Jump(JumpForce)
            this.JumpCooldown = JumpTime
        end
    end

    if (Input.GetKey("left")) then
        if (this.OnGround) then
            this:Move(Speed, -1.0)
        else
            this:Move(AirSpeed, -1.0)
        end
    end

    if (Input.GetKey("right")) then
        if (this.OnGround) then
            this:Move(Speed, 1.0)
        else
            this:Move(AirSpeed, 1.0)
        end
    end

    if (Input.GetKeyDown("f")) then
        if (this.AttackCooldown == 0.0) then
            this:Attack(AttackRange, AttackPower)
            this.AttackCooldown = AttackTime
        end
    end
end