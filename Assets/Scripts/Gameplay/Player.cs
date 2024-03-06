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
        public bool OnGround;
        public float AttackCooldown = 0.0f;
        public float JumpCooldown = 0.0f;

        private Vector3 m_Facing = new(1.0f, 0.0f);
        private Collider2D m_Collider;

        public ContactFilter2D TerrainContactFilter;

        private void Start()
        {
            m_Collider = GetComponent<Collider2D>();

            var scriptBehaviour = gameObject.AddComponent<LuaBehaviour>();
            scriptBehaviour.OnAssetUpdated += ScriptBehaviour_OnAssetUpdated;
            scriptBehaviour.Script = ResourcesManager.Get<LuaScript>("Scripts/PlayerController");
        }

        private void ScriptBehaviour_OnAssetUpdated(LuaInstance instance)
        {
            instance["this"] = this;
        }

        private void Update()
        {
            var hits = new RaycastHit2D[1];
            OnGround = m_Collider.Cast(Vector2.down, TerrainContactFilter, hits, 0.2f) > 0;

            AttackCooldown -= Time.deltaTime;
            AttackCooldown = math.max(0.0f, AttackCooldown);

            JumpCooldown -= Time.deltaTime;
            JumpCooldown = math.max(0.0f, JumpCooldown);
        }

        public void Jump(float force)
        {
            var rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.velocity = Vector2.up * force;
        }

        public void Move(float speed, float direction)
        {
            m_Facing = (Vector2.right * direction).normalized;

            Vector2 translation = speed * Time.deltaTime * m_Facing;
            var hits = new RaycastHit2D[1];

            if (m_Collider.Cast(m_Facing, TerrainContactFilter, hits, speed * Time.deltaTime) > 0)
            {
                Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));
                translation = hits[0].distance * m_Facing;
            }

            transform.Translate(translation);
        }

        public void Attack(float range, float power)
        {
            var hits = Physics2D.LinecastAll(transform.position, transform.position + (m_Facing * range));
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
