using Cgw.Assets;
using Cgw.Scripting;
using System;
using UnityEngine;

namespace Cgw.Gameplay
{
    public class GhostController : Enemy
    {
        public float Opacity = 1.0f;
        public float AttackCooldown = 0.0f;

        private Collider2D m_Collider;
        private LuaInstance m_Instance;

        public void Start()
        {
            m_Collider = GetComponent<Collider2D>();

            var scriptBehaviour = gameObject.AddComponent<LuaBehaviour>();
            scriptBehaviour.OnAssetUpdated += ScriptBehaviour_OnAssetUpdated;
            scriptBehaviour.Script = ResourcesManager.Get<LuaScript>("Scripts/Enemy/Ghost");
        }

        private void ScriptBehaviour_OnAssetUpdated(LuaInstance instance)
        {
            m_Instance = instance;

            instance["this"] = this;
        }

        public void Move(Vector2 direction, float speed)
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
    }

}