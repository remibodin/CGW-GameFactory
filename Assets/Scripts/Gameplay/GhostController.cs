using Cgw.Assets;
using Cgw.Scripting;
using System;
using UnityEngine;

namespace Cgw.Gameplay
{
    public class GhostController : Enemy
    {
        public float Opacity = 1.0f;
        public float ChargeCountdown = 0.0f;

        private Collider2D m_Collider;
        private LuaInstance m_Instance;

        public void Start()
        {
            m_Collider = GetComponent<Collider2D>();

            var scriptBehaviour = gameObject.AddComponent<LuaBehaviour>();
            scriptBehaviour.OnAssetUpdated += ScriptBehaviour_OnAssetUpdated;
            scriptBehaviour.Script = ResourcesManager.Get<LuaScript>("Scripts/Enemy/GhostController");
        }

        public void Update()
        {
            ChargeCountdown -= Time.deltaTime;
            ChargeCountdown = MathF.Max(ChargeCountdown, 0.0f);
        }

        private void ScriptBehaviour_OnAssetUpdated(LuaInstance instance)
        {
            m_Instance = instance;

            instance["this"] = this;
        }

        public void Move(Vector3 direction, float speed)
        {
            transform.Translate(speed * Time.deltaTime * direction);
        }

        public override void Attacked(float power)
        {
            m_Instance.Call("Attacked", power);
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