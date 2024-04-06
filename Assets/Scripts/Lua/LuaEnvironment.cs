using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using NLua;
using System;

namespace Cgw.Scripting
{
    public class LuaEnvironment 
    {
        public static readonly LuaEnvironment Instance = new();
        private static Dictionary<string, object> s_GlobalEnv = new Dictionary<string, object>();

        public static event Action OnEnvironmentUpdated;

        public static void AddEnvItem(string key, object value)
        {
            s_GlobalEnv[key] = value;
            if (OnEnvironmentUpdated != null)
                OnEnvironmentUpdated();
        }

        public static void RemoveEnvItem(string key)
        {
            s_GlobalEnv[key] = null;
            if (OnEnvironmentUpdated != null)
                OnEnvironmentUpdated();
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
