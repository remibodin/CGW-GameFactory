using UnityEngine;

using Cgw.Assets;

namespace Cgw.Scripting
{
    public class LuaBehaviour : MonoBehaviour
    {
        private LuaScript m_script;
        private LuaInstance m_instance;

        public LuaScript Script
        {
            get { return m_script; }
            set
            {
                if (value == null)
                {
                    Debug.LogError("LuaScript cant be null");
                    return;
                }
                if (m_script == value)
                {
                    return;
                }
                InitScript(value);
                InitTable();
            }
        }

        private void InitScript(LuaScript p_script)
        {
            if (m_script != null)
            {
                m_script.OnUpdated -= ScriptUpdated;
            }
            m_script = p_script;
            m_script.OnUpdated += ScriptUpdated;
            m_instance = m_script.CreateInstance();
        }

        private void InitTable()
        {
            m_instance["transform"] = transform;
        }

        private void ScriptUpdated(Asset p_newScript)
        {
            Script = p_newScript as LuaScript;
        }

        private void Update()
        {
            m_instance.Call("Update");
        }
    }
}