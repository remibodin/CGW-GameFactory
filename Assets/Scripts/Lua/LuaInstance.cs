using UnityEngine;

using NLua;
using System.Collections.Generic;

namespace Cgw.Scripting
{
    public class LuaInstance
    {
        private Lua m_environment;
        private Dictionary<string, LuaFunction> m_functions = new Dictionary<string, LuaFunction>();

        public object this[string key]
        {
            get
            {
                if (m_environment != null)
                {
                    return m_environment[key];
                }
                return null;
            }
            set
            {
                if (m_environment != null)
                {
                    m_environment[key] = value;
                }
            }
        }

        public bool InitFromSource(string source)
        {
            InitEnvironment();

            try
            {
                m_environment.DoString(source);
                return true;
            }
            catch (NLua.Exceptions.LuaException e)
            {
                Debug.LogError(FormatException(e));
                return false;
            }
        }

        public bool InitFromFile(string fileName)
        {
            InitEnvironment();

            try
            {
                m_environment.DoFile(fileName);
                return true;
            }
            catch (NLua.Exceptions.LuaException e)
            {
                Debug.LogError(FormatException(e));
                return false;
            }
        }

        public object[] Call(string function, params object[] args)
        {
            object[] result = new object[0];
            if (m_environment == null)
            {
                return result;
            }

            LuaFunction luaFunction = null;
            if (m_functions.ContainsKey(function))
            {
                luaFunction = m_functions[function];
            }
            else
            {
                luaFunction = m_environment.GetFunction(function);
                if (luaFunction == null)
                {
                    return result;
                }

                m_functions[function] = luaFunction;
            }

            try
            {
                // Note: calling a function that does not
                // exist does not throw an exception.
                if (args != null)
                {
                    result = luaFunction.Call(args);
                }
                else
                {
                    result = luaFunction.Call();
                }
            }
            catch (NLua.Exceptions.LuaException e)
            {
                Debug.LogError(FormatException(e));
            }

            return result;
        }

        public object[] Call(string function)
        {
            return Call(function, null);
        }

        private void InitEnvironment()
        {
            m_environment?.Close();
            m_functions?.Clear();
            m_environment = new Lua();
            m_environment.LoadCLRPackage();
        }

        public static string FormatException(NLua.Exceptions.LuaException e)
        {
            string source = (string.IsNullOrEmpty(e.Source)) ? "<no source>" : e.Source.Substring(0, e.Source.Length - 2);
            return string.Format("{0}\nLua (at {2})", e.Message, string.Empty, source);
        }
    }
}