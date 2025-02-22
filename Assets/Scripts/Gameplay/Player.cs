using System;
using System.Linq;
using UnityEngine.InputSystem;

using UnityEngine;
using Unity.Mathematics;

using FMODUnity;
using FMOD.Studio;

using Cgw.Audio;
using System.Collections;
using Unity.Collections;

namespace Cgw.Gameplay
{
    public class Player : SingleBehaviour<Player>
    {
        public float Life = 3.0f;
        public float Speed = 0.65f;
        public float AirSpeed = 1.0f;
        public float JumpCooldownTime = .85f;
        public float JumpForce = 3.5f;

        public float LaunchCooldownTime = 6.0f;
        public float AttackCooldownTime = 0.6f;
        public float DamageCooldownTime = 0.8f;

        public float AttackRange = 0.8f;
        public float AttackPower = 1.0f;
        public float SinceLastFootStep = 0f;

        public float KillY = -50.0f;

        private float PreviousXPosition = 0.0f;
        private ESurfaceType PreviousOnMaterial = ESurfaceType.Dirt;
        private bool PreviousOnGround = true;
        private bool NoControl = false;
        private float Inertia = 0.0f;
        private ContactFilter2D TerrainContactFilter;

        private float AttackTimer = 0.0f;
        private float JumpTimer = 0.0f;
        private float DamageTimer = 0.0f;
        private float LaunchTimer = 0.0f;

        public Vector3 Facing = new(1.0f, 0.0f);
        public float MinSurfaceAngle = 0.4f;
        public Vector2 Motion;

        public bool OnGround { get; private set; }
        public ESurfaceType OnMaterial { get; private set; }

        [ReadOnly]
        public InputActionAsset InputActions;

        private Collider2D m_Collider;
        private Rigidbody2D m_Rigidbody;
        private EventInstance m_footStepEventInstance;
        private EventInstance m_landingEventInstance;
        private EventReference FootStepEventRef;
        private EventReference LandingEventRef;
        private EventReference AttackEventRef;

        private InputAction m_HorizontalAction = null;

        protected override void Awake()
        {
            base.Awake();

            InputActions.Enable();

            m_HorizontalAction = InputActions.FindActionMap("Player").FindAction("Horizontal");

            InputActions.FindActionMap("Player").FindAction("Jump").performed += Player_OnJump;
            InputActions.FindActionMap("Player").FindAction("Attack").performed += Player_OnAttack;
            InputActions.FindActionMap("Player").FindAction("Interact").performed += Player_OnInteract;
            InputActions.FindActionMap("Player").FindAction("AragnaAttack").performed += Player_OnAragnaAttack;
        }

        public IEnumerator Die()
        {
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
            Destroy(SpiderController.Instance);
            PlayerSpawner.Instance.RequestSpawn();
        }

        public void DieImmediately()
        {
            Destroy(gameObject);
            Destroy(SpiderController.Instance);
            PlayerSpawner.Instance.RequestSpawn();
        }

        public IEnumerator TookDamage()
        {
            yield return new WaitForSeconds(0.5f);
            if (Life <= 0.0f)
            {
                DieImmediately();
            }
            NoControl = false;
        }

        public void TakeDamage(float power, Enemy enemy)
        {
            if (Mathf.Approximately(DamageTimer, 0.0f))
            {
                NoControl = true;
                Life = Life - power;
                DamageTimer = DamageCooldownTime;
                var directionFromEnemy = (transform.position - enemy.transform.position).normalized;
                m_Rigidbody.AddForce((Vector3.up + directionFromEnemy) * 3.0f);
                CoroutineRunner.StartCoroutine(TookDamage());
            }
        }

        private void Player_OnAragnaAttack(InputAction.CallbackContext context)
        {
            if (Mathf.Approximately(LaunchTimer, 0.0f))
            {
                SpiderController.Instance.Launch();
                LaunchTimer = LaunchCooldownTime;
            }
        }

        private void Player_OnInteract(InputAction.CallbackContext context)
        {
        }

        private void Player_OnAttack(InputAction.CallbackContext context)
        {
            if (Mathf.Approximately(AttackTimer, 0.0f))
            {
                Attack(AttackRange, AttackPower);
                AttackTimer = AttackCooldownTime;
            }
        }

        private void Player_OnJump(InputAction.CallbackContext obj)
        {
            if (OnGround)
            {
                if (Mathf.Approximately(JumpTimer, 0.0f))
                {
                    Inertia = Motion.x;
                    Jump(JumpForce);
                    JumpTimer = JumpCooldownTime;
                }
            }
        }

        public void Start()
        {
            m_Collider = GetComponent<Collider2D>();
            m_Rigidbody = GetComponent<Rigidbody2D>();

            m_footStepEventInstance = RuntimeManager.CreateInstance(FootStepEventRef);
            RuntimeManager.AttachInstanceToGameObject(m_footStepEventInstance, transform);

            m_landingEventInstance = RuntimeManager.CreateInstance(LandingEventRef);
            RuntimeManager.AttachInstanceToGameObject(m_landingEventInstance, transform);

            PreviousOnGround = OnGround;
            PreviousOnMaterial = OnMaterial;
        }

        protected void OnDestroy()
        {
            m_footStepEventInstance.release();
            m_landingEventInstance.release();

            InputActions.FindActionMap("Player").FindAction("Jump").performed -= Player_OnJump;
            InputActions.FindActionMap("Player").FindAction("Attack").performed -= Player_OnAttack;
            InputActions.FindActionMap("Player").FindAction("Interact").performed -= Player_OnInteract;
            InputActions.FindActionMap("Player").FindAction("AragnaAttack").performed -= Player_OnAragnaAttack;
        }

        public float GetHorizontalInput()
        {
            return m_HorizontalAction.ReadValue<float>();
        }

        public void Update()
        {
            Motion = Vector2.zero;

            var hits = Physics2D.RaycastAll(transform.position, Vector2.down, 0.2f);
            var hasHit = hits.Any(x => !x.collider.CompareTag("Player") && !x.collider.CompareTag("Spider"));
            if (hasHit)
            {
                var hit = hits.First(x => !x.collider.CompareTag("Player") && !x.collider.CompareTag("Spider"));  
                var hitNormal = hit.normal;
                OnGround = Mathf.Abs(hitNormal.y) > MinSurfaceAngle;
                OnMaterial = hit.collider.GetSurfaceType();
            }
            else
            {
                OnGround = false;
                OnMaterial = ESurfaceType.Unknown;
            }

            AttackTimer -= Time.deltaTime;
            AttackTimer = math.max(0.0f, AttackTimer);

            JumpTimer -= Time.deltaTime;
            JumpTimer = math.max(0.0f, JumpTimer);

            DamageTimer -= Time.deltaTime;
            DamageTimer = math.max(0.0f, DamageTimer);

            LaunchTimer -= Time.deltaTime;
            LaunchTimer = math.max(0.0f, LaunchTimer);

            if (OnGround && !PreviousOnGround)
            {
                Inertia = 0.0f;
            }

            if (!NoControl)
            {
                var horizontalAxis = GetHorizontalInput();
                if (OnGround)
                {
                    if (Mathf.Abs(horizontalAxis) > 0.0f)
                    {
                        if (Mathf.Approximately(JumpTimer, 0.0f))
                        {
                            Move(Speed, horizontalAxis);
                        }
                    }
                }
                else
                {
                    MoveWithInertia(AirSpeed, horizontalAxis, Inertia);
                }
            }

            if (transform.position.y <= KillY)
            {
                DieImmediately();
            }

            PreviousOnGround = OnGround;
            PreviousOnMaterial = OnMaterial;
        }

        public void Jump(float force)
        {
            var rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.linearVelocity = Vector2.up * force;
        }

        public void Move(float speed, float direction)
        {
            Facing = (Vector2.right * direction).normalized;

            Vector2 translation = speed * Time.deltaTime * Facing;
            var hits = new RaycastHit2D[1];

            if (m_Collider.Cast(Facing, TerrainContactFilter, hits, speed * Time.deltaTime) > 0)
            {
                Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));
                translation = hits[0].distance * Facing;
            }

            transform.Translate(translation);

            Motion += translation;
        }

        public void MoveWithInertia(float speed, float direction, float inertia)
        {
            Facing = (Vector2.right * direction).normalized;

            Vector2 translation = speed * Time.deltaTime * Facing;
            var hits = new RaycastHit2D[1];

            if (m_Collider.Cast(Facing, TerrainContactFilter, hits, speed * Time.deltaTime) > 0)
            {
                Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));
                translation = hits[0].distance * Facing;
            }

            transform.Translate(translation + Vector2.right * inertia);

            Motion += translation;
        }

        public void Attack(float range, float power)
        {
            var animator = GetComponent<PlayerAnimator>();
            if (animator != null)
            {
                animator.Attack();
            }

            var hits = Physics2D.LinecastAll(transform.position + Vector3.up, transform.position + (Facing * range) + Vector3.up);
            foreach (var hit in hits)
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    if (hit.collider.gameObject.TryGetComponent<Enemy>(out var enemy))
                    {
                        enemy.Attacked(power);
                    }
                }
            }
        }

        public void OnAnimEvent(string animEvent)
        {
            if (animEvent == "HeroStep")
            {
                m_footStepEventInstance.setParameterByName("Material", (float)OnMaterial);
                m_footStepEventInstance.start();
            }
            else if (animEvent == "HeroLanding")
            {
                m_landingEventInstance.setParameterByName("Material", (float)OnMaterial);
                m_landingEventInstance.start();
                AudioManager.Instance.PlayRandom("Sounds/Collections/JumpDirt");
            }
            else if (animEvent == "HeroAttack")
            {
                AudioManager.Instance.Play("Sounds/HERO_ATTAQUE_WHOOSH-05_1");
            }
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Enemy"))
            {
                var enemy = collision.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.OnCollisionWithPlayer();
                }
            }
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                var enemy = collision.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.OnCollisionWithPlayer();
                }
            }
        }
    }
}
