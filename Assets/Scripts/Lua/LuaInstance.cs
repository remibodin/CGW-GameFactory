using UnityEngine;

using NLua;

namespace Cgw.Scripting
{
    public class LuaInstance
    {
        private Lua m_environment;

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
            object[] result = new System.Object[0];
            if (m_environment == null)
            {
                return result;
            }
            
            LuaFunction lf = m_environment.GetFunction(function);
            if (lf == null)
            {
                return result;
            }
            try
            {
                // Note: calling a function that does not
                // exist does not throw an exception.
                if (args != null)
                {
                    result = lf.Call(args);
                }
                else
                {
                    result = lf.Call();
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