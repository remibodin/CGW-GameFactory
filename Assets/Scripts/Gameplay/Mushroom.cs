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
        public float SpiderTouchTimer = 0.0f;

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
            scriptBehaviour.OnAssetUpdated += OnAssetUpdate;
            scriptBehaviour.Script = ResourcesManager.Get<LuaScript>("Scripts/Enemy/MushroomController");
        }

        public void Update()
        {
            SpiderTouchTimer -= Time.deltaTime;
            SpiderTouchTimer = Mathf.Max(SpiderTouchTimer, 0.0f);
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

        protected override void OnAssetUpdate(LuaInstance instance)
        {
            base.OnAssetUpdate(instance);

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

        public override void OnCollisionWithDanger()
        {
            m_Instance.Call("OnCollisionWithDanger");
        }
    }
}
