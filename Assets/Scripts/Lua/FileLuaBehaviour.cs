using System.IO;
using UnityEngine;

public abstract class FileLuaBehaviour : BaseLuaBehaviour
{
    public string m_FileName;

    private FileSystemWatcher m_FileSystemWatcher;

    public FileLuaBehaviour()
    {
        m_LuaScript.InitFromFile(m_FileName);

        m_FileSystemWatcher = new FileSystemWatcher(Path.GetDirectoryName(m_FileName));
        m_FileSystemWatcher.Filter = Path.GetFileName(m_FileName);
        m_FileSystemWatcher.Changed += OnScriptModified;
        m_FileSystemWatcher.EnableRaisingEvents = true;
    }

    public sealed override void InitScript()
    {
        m_LuaScript.InitFromFile(m_FileName);
    }

    private void OnScriptModified(object sender, FileSystemEventArgs e)
    {
        InitScript();
        InitTable();
    }

    public new void Awake()
    {
        base.Awake();
    }
}
