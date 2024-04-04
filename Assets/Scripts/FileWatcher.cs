using System;
using System.IO;

namespace Cgw
{
    public class FileWatcher
    {
        private readonly Action<string> m_onChangeHandler;

        public FileWatcher(string p_path, Action<string> p_onChangeHandler)
        {
            m_onChangeHandler = p_onChangeHandler;

            if (!Directory.Exists(p_path))
            {
                UnityEngine.Debug.LogError($"Directory not found {p_path}");
                return;
            }

            var watcher = new FileSystemWatcher()
            {
                Path = p_path,
                NotifyFilter = NotifyFilters.LastWrite,
                IncludeSubdirectories = true
            };

            watcher.Changed += OnChanged;
            watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object _, FileSystemEventArgs p_e)
        {
            m_onChangeHandler(p_e.FullPath);
        }
    }
}