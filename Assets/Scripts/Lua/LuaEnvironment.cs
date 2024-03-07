using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using NLua;

namespace Cgw.Scripting
{
    public class LuaEnvironment 
    {
        public static readonly LuaEnvironment Instance = new();
        private static Dictionary<string, object> s_GlobalEnv = new Dictionary<string, object>();

        public static void AddEnvItem(string key, object value)
        {
            s_GlobalEnv[key] = value;
        }

        public static void RemoveEnvItem(string key)
        {
            if (s_GlobalEnv.ContainsKey(key))
            {
                s_GlobalEnv.Remove(key);
            }
        }

        public static void InjectEnv(LuaInstance instance)
        {
            foreach (var envItem in s_GlobalEnv)
            {
                instance[envItem.Key] = envItem.Value;
            }
        }
    }
}
