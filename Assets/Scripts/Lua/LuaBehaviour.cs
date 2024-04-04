using UnityEngine;

using Cgw.Assets;
using Cgw.Graphics;
using System;

namespace Cgw.Scripting
{
    public class LuaBehaviour : AssetBehaviour<LuaScript>
    {
        public LuaScript Script
        {
            get { return Asset; }
            set { Asset = value; }
        }

        public event Action<LuaInstance> OnAssetUpdated;

        private LuaInstance m_instance;

        protected override void AssetUpdated()
        {
            m_instance = Script.CreateInstance();
            InitTable();
            if (OnAssetUpdated != null)
            {
                OnAssetUpdated(m_instance);
            }
            m_instance.Call("Start");
        }

        private void InitTable()
        {
            LuaEnvironment.InjectEnv(m_instance);

            m_instance["transform"] = transform;
        }

        private void Update()
        {
            m_instance.Call("Update");
        }

        private void Start()
        {
            m_instance.Call("Start");
        }

        private void LateUpdate()
        {
            m_instance.Call("LateUpdate");
        }

        private void OnDestroy()
        {
            m_instance.Call("OnDestroy");
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Collider2D>().CompareTag("Player"))
            {
                m_instance.Call("CollisionWithPlayer");
            }
        }
    }
}