using System;
using System.Collections;
using System.Collections.Generic;

using Unity.Mathematics;
using UnityEngine;

using Cgw.Assets;
using Cgw.Scripting;

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
        public Vector3 Facing = new(1.0f, 0.0f);
        public float MinSurfaceAngle = 0.8f;
        public Vector2 Motion;

        private Collider2D m_Collider;
        private LuaInstance m_Instance;

        private void Start()
        {
            m_Collider = GetComponent<Collider2D>();

            var scriptBehaviour = gameObject.AddComponent<LuaBehaviour>();
            scriptBehaviour.OnAssetUpdated += ScriptBehaviour_OnAssetUpdated;
            scriptBehaviour.Script = ResourcesManager.Get<LuaScript>("Scripts/PlayerController");
        }

        private void ScriptBehaviour_OnAssetUpdated(LuaInstance instance)
        {
            m_Instance = instance;

            instance["this"] = this;
        }

        public void TakeDamage(float power)
        {
            m_Instance.Call("TakeDamage", power);
        }

        private void Update()
        {
            Motion = Vector2.zero;

            var hits = new RaycastHit2D[1];
            if (m_Collider.Cast(Vector2.down, TerrainContactFilter, hits, 0.2f) > 0)
            {
                var hitNormal = hits[0].normal;
                OnGround = Mathf.Abs(hitNormal.y) > MinSurfaceAngle;
            }
            else
            {
                OnGround = false;
            }

            if (OnGround)
            {
                OnMaterial = hits[0].collider.tag;
            }
            else
            {
                OnMaterial = "Air";
            }

            AttackCooldown -= Time.deltaTime;
            AttackCooldown = math.max(0.0f, AttackCooldown);

            JumpCooldown -= Time.deltaTime;
            JumpCooldown = math.max(0.0f, JumpCooldown);

            DamageCooldown -= Time.deltaTime;
            DamageCooldown = math.max(0.0f, DamageCooldown);
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

        public void Attack(float range, float power)
        {
            var hits = Physics2D.LinecastAll(transform.position, transform.position + (Facing * range));
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
    }
}
