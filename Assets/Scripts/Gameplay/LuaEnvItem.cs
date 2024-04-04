using Cgw.Scripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cgw.Gameplay
{
    public abstract class LuaEnvItem : LuaMonoBehaviour
    {
        public string ObjectName = "";

        protected virtual void Awake()
        {
            Register();
        }

        protected virtual void OnDestroy()
        {
            Unregister();
        }

        private void Register()
        {
            LuaEnvironment.AddEnvItem(ObjectName, this);
        }

        private void Unregister()
        {
            LuaEnvironment.RemoveEnvItem(ObjectName);
        }
    }
}
