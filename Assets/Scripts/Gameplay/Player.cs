using System;
using System.Linq;
using UnityEngine.InputSystem;

using UnityEngine;
using Unity.Mathematics;

using FMODUnity;
using FMOD.Studio;

using Cgw.Assets;
using Cgw.Scripting;
using Cgw.Audio;

namespace Cgw.Gameplay
{
    public class Player : LuaEnvItem
    {
        public ContactFilter2D TerrainContactFilter;
        public float AttackCooldown = 0.0f;
        public float JumpCooldown = 0.0f;
        public float DamageCooldown = 0.0f;
        public float LaunchCooldown = 0.0f;
        public Vector3 Facing = new(1.0f, 0.0f);
        public float MinSurfaceAngle = 0.4f;
        public Vector2 Motion;
        public EventReference FootStepEventRef;
        public EventReference LandingEventRef;

        public bool OnGround { get; private set; }
        public ESurfaceType OnMaterial { get; private set; }

        public InputActionAsset InputActions;

        private Collider2D m_Collider;
        private EventInstance m_footStepEventInstance;
        private EventInstance m_landingEventInstance;

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

        private void Player_OnAragnaAttack(InputAction.CallbackContext context)
        {
            m_Instance.Call("OnAragnaAttack");
        }

        private void Player_OnInteract(InputAction.CallbackContext context)
        {
            m_Instance.Call("OnInteract");
        }

        private void Player_OnAttack(InputAction.CallbackContext context)
        {
            m_Instance.Call("OnAttack");
        }

        private void Player_OnJump(InputAction.CallbackContext obj)
        {
            m_Instance.Call("OnJump");
        }

        private void Start()
        {
            m_Collider = GetComponent<Collider2D>();

            var scriptBehaviour = gameObject.AddComponent<LuaBehaviour>();
            scriptBehaviour.OnAssetUpdated += OnAssetUpdate;
            scriptBehaviour.Script = ResourcesManager.Get<LuaScript>("Scripts/PlayerController");

            m_footStepEventInstance = RuntimeManager.CreateInstance(FootStepEventRef);
            RuntimeManager.AttachInstanceToGameObject(m_footStepEventInstance, transform);

            m_landingEventInstance = RuntimeManager.CreateInstance(LandingEventRef);
            RuntimeManager.AttachInstanceToGameObject(m_landingEventInstance, transform);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            m_footStepEventInstance.release();
            m_landingEventInstance.release();
        }

        protected override void OnAssetUpdate(LuaInstance instance)
        {
            base.OnAssetUpdate(instance);

            instance["this"] = this;
        }

        public float GetHorizontalInput()
        {
            return m_HorizontalAction.ReadValue<float>();
        }

        public void TakeDamage(float power, Enemy enemy)
        {
            m_Instance.Call("TakeDamage", power, enemy);
        }

        private void Update()
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

            AttackCooldown -= Time.deltaTime;
            AttackCooldown = math.max(0.0f, AttackCooldown);

            JumpCooldown -= Time.deltaTime;
            JumpCooldown = math.max(0.0f, JumpCooldown);

            DamageCooldown -= Time.deltaTime;
            DamageCooldown = math.max(0.0f, DamageCooldown);

            LaunchCooldown -= Time.deltaTime;
            LaunchCooldown = math.max(0.0f, LaunchCooldown);
        }

        public void Jump(float force)
        {
            var rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.velocity = Vector2.up * force;
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

        protected override void OnAnimEvent(string animEvent)
        {
            if (animEvent == "HeroStep")
            {
                m_footStepEventInstance.setParameterByName("Material", (float)OnMaterial);
                m_footStepEventInstance.start();
            }
            if (animEvent == "HeroLanding")
            {
                m_landingEventInstance.setParameterByName("Material", (float)OnMaterial);
                m_landingEventInstance.start();
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
