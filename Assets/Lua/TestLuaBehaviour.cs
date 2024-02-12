using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLuaBehaviour : SourceLuaBehaviour
{
    public override void InitTable()
    {
        m_LuaScript["this"] = this;
        m_LuaScript["transform"] = transform;
    }

    void Start()
    {
        m_LuaScript.Call("Start");
    }

    void Update()
    {
        m_LuaScript.Call("Update");
    }
}
