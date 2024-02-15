using UnityEngine;

using Cgw.Assets;
using Cgw.Graphics;

namespace Cgw.Scripting
{
    public class LuaBehaviour : AssetBehaviour<LuaScript>
    {
        public LuaScript Script
        {
            get { return Asset; }
            set { Asset = value; }
        }

        private LuaInstance m_instance;

        protected override void AssetUpdated()
        {
            m_instance = Script.CreateInstance();
            InitTable();
        }

        private void InitTable()
        {
            m_instance["transform"] = transform;
        }

        private void Update()
        {
            m_instance.Call("Update");
        }
    }
}