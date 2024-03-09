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
        public ContactFilter2D ContactFilter;
        public float TouchTime = 3.0f;
        public float TouchSpeedMultiplier = 0.3f;

        private LuaInstance m_Instance;

        void Start()
        {
            var scriptBehaviour = gameObject.AddComponent<LuaBehaviour>();
            scriptBehaviour.OnAssetUpdated += ScriptBehaviour_OnAssetUpdated;
            scriptBehaviour.Script = ResourcesManager.Get<LuaScript>("Scripts/SpiderController");
        }

        private void ScriptBehaviour_OnAssetUpdated(LuaInstance instance)
        {
            m_Instance = instance;

            instance["this"] = this;
        }

        public void MoveFollow(Vector3 destination, float speed)
        {
            transform.position = Vector3.Lerp(transform.position, destination, speed * Time.deltaTime);
        }

        public void MoveTowards(Vector3 destination, float speed)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        }

        public Enemy CheckLaunch(Vector3 origin, Vector3 direction, float range)
        {
            RaycastHit2D[] hits = new RaycastHit2D[1];
            if (Physics2D.Raycast(origin, direction, ContactFilter, hits, range) > 0)
            {
                return hits[0].collider.GetComponent<Enemy>();
            }
            return null;
        }

        public void Launch()
        {
            m_Instance.Call("Launch");
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Enemy"))
            {
                var enemy = collision.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    m_Instance.Call("OnCollisionWithEnemy");
                    enemy.OnCollisionWithSpider();
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
                    m_Instance.Call("OnCollisionWithEnemy");
                    enemy.OnCollisionWithSpider();
                }
            }
        }
    }
}
