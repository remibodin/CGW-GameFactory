import 'UnityEngine'

local Life = 3;
local Speed = 1.1;
local AirSpeed = 1.7;
local JumpTime = .85;
local JumpForce = 7;
local LaunchTimer = 6.0;

local AttackTime = 1.0;
local DamageTime = 0.8;
local AttackRange = 0.8;
local AttackPower = 1.0;

local SinceLastFootStep = 0;

local PreviousOnMaterial = "Dirt"
local PreviousOnGround = true
local NoControl = false

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

function PlayAttackSound()
    AudioManager:Play("Sounds/HERO_ATTAQUE_WHOOSH-05_1")
end

function Update()
    -- if (not this.OnGround == PreviousOnGround and PreviousOnGround == false) then
    --     Debug.Log("drop")
    --     AudioManager:PlayRandom("Sounds/Collections/JumpDirt")
    -- end

    if (NoControl) then
        PreviousOnGround = this.OnGround
        PreviousOnMaterial = this.OnMaterial
        return
    end

    if (this.OnMaterial == "Slope" and PreviousOnMaterial ~= "Slope") then
        Debug.Log("OnSlope")
        this.JumpCooldown = 0.0
    end

    local jumpAxis = Input.GetAxis("Jump")
    if (jumpAxis > 0 and (this.OnGround or this.OnMaterial == "Slope")) then
        if (this.JumpCooldown == 0.0) then
            this:Jump(JumpForce * jumpAxis)
            AudioManager:PlayRandom("Sounds/Collections/JumpDirt")
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
            this:DelayAction(0.32, "PlayAttackSound")
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

    PreviousOnGround = this.OnGround
    PreviousOnMaterial = this.OnMaterial
end
