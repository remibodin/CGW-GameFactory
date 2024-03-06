using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using NLua;

namespace Cgw.Scripting
{
    public class LuaEnvironment 
    {
        public static readonly LuaEnvironment Instance = new();
        private Lua m_Environment = new Lua();

        public static Lua GetEnvironment()
        {
            return Instance.m_Environment;
        }
    }
}
