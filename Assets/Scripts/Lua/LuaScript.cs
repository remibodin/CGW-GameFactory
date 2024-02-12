using NLua;
using UnityEngine;

public class LuaScript
{
    private Lua m_Environment;

    public object this[string key]
    {
        get
        {
            if (m_Environment != null)
            {
                return m_Environment[key];
            }
            return null;
        }
        set
        {
            if (m_Environment != null)
            {
                m_Environment[key] = value;
            }
        }
    }

    public bool InitFromSource(string source)
    {
        m_Environment?.Close();
        m_Environment = new Lua();
        m_Environment.LoadCLRPackage();

        try
        {
            //result = env.DoString(source);
            m_Environment.DoString(source);
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
        m_Environment?.Close();
        m_Environment = new Lua();
        m_Environment.LoadCLRPackage();

        try
        {
            m_Environment.DoFile(fileName);
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
        if (m_Environment == null) return result;
        LuaFunction lf = m_Environment.GetFunction(function);
        if (lf == null) return result;
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

    public static string FormatException(NLua.Exceptions.LuaException e)
    {
        string source = (string.IsNullOrEmpty(e.Source)) ? "<no source>" : e.Source.Substring(0, e.Source.Length - 2);
        return string.Format("{0}\nLua (at {2})", e.Message, string.Empty, source);
    }
}
