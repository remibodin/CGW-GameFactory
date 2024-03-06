using Cgw.Scripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cgw.Gameplay
{
    public abstract class LuaEnvItem : MonoBehaviour
    {
        public string ObjectName = "";

        protected virtual void Awake()
        {
            Register();
        }

        private void Register()
        {
            LuaEnvironment.GetEnvironment()[ObjectName] = this;
        }
    }
}
