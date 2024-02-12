using UnityEngine;

public abstract class SourceLuaBehaviour : BaseLuaBehaviour
{
    public string Source
    {
        get { return m_Source; }
        set
        {
            m_Source = value;
            InitScript();
            InitTable();
        }
    }

    [SerializeField, Multiline]
    private string m_Source;

    public sealed override void InitScript()
    {
        m_LuaScript.InitFromSource(Source);
    }

    public new void Awake()
    {
        base.Awake();
    }
}
