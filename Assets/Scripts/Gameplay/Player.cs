using System;
using System.Collections;
using System.Collections.Generic;

using Unity.Mathematics;
using UnityEngine;

using Cgw.Assets;
using Cgw.Scripting;
using System.Linq;

namespace Cgw.Gameplay
{
    public class Player : LuaEnvItem
    {
        public ContactFilter2D TerrainContactFilter;
        
        public bool OnGround;
        public string OnMaterial;
        public float AttackCooldown = 0.0f;
        public float JumpCooldown = 0.0f;
        public float DamageCooldown = 0.0f;
        public float LaunchCooldown = 0.0f;
        public Vector3 Facing = new(1.0f, 0.0f);
        public float MinSurfaceAngle = 0.4f;
        public Vector2 Motion;

        private Collider2D m_Collider;

        private void Start()
        {
            m_Collider = GetComponent<Collider2D>();

            var scriptBehaviour = gameObject.AddComponent<LuaBehaviour>();
            scriptBehaviour.OnAssetUpdated += OnAssetUpdate;
            scriptBehaviour.Script = ResourcesManager.Get<LuaScript>("Scripts/PlayerController");
        }

        protected override void OnAssetUpdate(LuaInstance instance)
        {
            base.OnAssetUpdate(instance);

            instance["this"] = this;
        }

        public void TakeDamage(float power, Enemy enemy)
        {
            m_Instance.Call("TakeDamage", power, enemy);
        }

        private void Update()
        {
            Motion = Vector2.zero;

            var hits = Physics2D.RaycastAll(transform.position, Vector2.down, 0.2f);
            var hasHit = hits.Any(x => !x.collider.CompareTag("Player"));
            if (hasHit)
            {
                var hit = hits.First(x => !x.collider.CompareTag("Player"));  
                var hitNormal = hit.normal;
                OnGround = Mathf.Abs(hitNormal.y) > MinSurfaceAngle;
                OnMaterial = hit.collider.tag;
            }
            else
            {
                OnGround = false;
                OnMaterial = "Air";
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
