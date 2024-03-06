using NLua;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace Cgw.Scripting
{
    public class LuaEnvironment : MonoBehaviour
    {
        private static LuaEnvironment Instance;

        private Lua m_Environment = new();

        public LuaEnvironment()
        {
            Instance = this;
        }

        public static Lua GetEnvironment()
        {
            return Instance.m_Environment;
        }
    }
}
