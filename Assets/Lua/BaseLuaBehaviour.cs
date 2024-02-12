using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseLuaBehaviour : MonoBehaviour
{
    public abstract void InitTable();
    public abstract void InitScript();

    protected LuaScript m_LuaScript = new LuaScript();

    public void Awake()
    {
        InitScript();
        InitTable();
    }
}
