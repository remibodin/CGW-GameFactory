using Cgw.Assets;
using Cgw.Scripting;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace Cgw.Gameplay
{
    public class Mushroom : Enemy
    {
        public ContactFilter2D TerrainContactFilter;
        public float AttackCooldown = 0.0f;

        private LuaInstance m_Instance;
        private Collider2D m_Collider;
        private Vector2 m_Facing;

        public override void Attacked(float power)
        {
            m_Instance.Call("Attacked", power);
        }

        void Start()
        {
            m_Collider = GetComponent<Collider2D>();

            var scriptBehaviour = gameObject.AddComponent<LuaBehaviour>();
            scriptBehaviour.OnAssetUpdated += ScriptBehaviour_OnAssetUpdated;
            scriptBehaviour.Script = ResourcesManager.Get<LuaScript>("Scripts/Enemy/Mushroom");
        }

        public void Update()
        {
            AttackCooldown -= Time.deltaTime;
            AttackCooldown = math.max(0.0f, AttackCooldown);
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

        private void ScriptBehaviour_OnAssetUpdated(LuaInstance instance)
        {
            m_Instance = instance;
            instance["this"] = this;
        }

        public override void OnCollisionWithPlayer()
        {
            m_Instance.Call("OnCollisionWithPlayer");
        }

        public override void OnCollisionWithSpider()
        {
            m_Instance.Call("OnCollisionWithSpider");
        }
    }
}
