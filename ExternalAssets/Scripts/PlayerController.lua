import 'UnityEngine'

local Life = 3;
local Speed = 1.5;
local AirSpeed = 2.5;
local AttackRange = 1.5;
local AttackPower = 1.0;
local JumpTime = 0.85;
local AttackTime = 1.0;
local JumpForce = 8.0;

function Update()
    if (Input.GetKeyDown("space") and this.OnGround) then
        if this.JumpCooldown == 0.0 then
            this:Jump(JumpForce)
            this.JumpCooldown = JumpTime
        end
    end

    if (this.OnGround and this.JumpCooldown == 0.0) then
        if (Input.GetKey("left")) then
            this:Move(Speed, -1.0)
        end
        if (Input.GetKey("right")) then
            this:Move(Speed, 1.0)
        end
    end

    if (this.OnGround == false) then
        if (Input.GetKey("left")) then
            this:Move(AirSpeed, -1.0)
        end
        if (Input.GetKey("right")) then
            this:Move(AirSpeed, 1.0)
        end
    end

    -- if (Input.GetKey("left")) then
    --     if (this.OnGround and this.JumpCooldown == 0.0) then
    --         this:Move(Speed, -1.0)
    --     else if (this.OnGround == false) then
    --         this:Move(AirSpeed, -1.0)
    --     end
    -- end

    -- if (Input.GetKey("right")) then
    --     if (this.OnGround and this.JumpCooldown == 0.0) then
    --         this:Move(Speed, 1.0)
    --     else if (this.OnGround == false) then
    --         this:Move(AirSpeed, 1.0)
    --     end
    -- end

    if (Input.GetKeyDown("f")) then
        if (this.AttackCooldown == 0.0) then
            this:Attack(AttackRange, AttackPower)
            this.AttackCooldown = AttackTime
        end
    end
end