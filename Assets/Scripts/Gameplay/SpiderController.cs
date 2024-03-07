using Cgw.Assets;
using Cgw.Scripting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cgw.Gameplay
{
    public class SpiderController : LuaEnvItem
    {
        void Start()
        {
            var scriptBehaviour = gameObject.AddComponent<LuaBehaviour>();
            scriptBehaviour.OnAssetUpdated += ScriptBehaviour_OnAssetUpdated;
            scriptBehaviour.Script = ResourcesManager.Get<LuaScript>("Scripts/SpiderController");
        }

        private void ScriptBehaviour_OnAssetUpdated(LuaInstance instance)
        {
            instance["this"] = this;
        }

        public void MoveFollow(Vector3 destination, float speed)
        {
            transform.position = Vector3.Lerp(transform.position, destination, speed * Time.deltaTime);
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                var enemy = collision.GetComponent<Enemy>();
                enemy.OnCollisionWithSpider();
            }
        }
    }
}
