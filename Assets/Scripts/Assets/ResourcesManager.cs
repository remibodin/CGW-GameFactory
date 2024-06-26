using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;

using Cgw.Assets.Loaders;

namespace Cgw.Assets
{
    public class ResourcesManager
    {
        private static Dictionary<Type, IAssetLoader> m_loaders = new Dictionary<Type, IAssetLoader>();
        private static Dictionary<string, Asset> m_resources = new Dictionary<string, Asset>();
        private static string m_projectRoot;
        private static FileWatcher m_fileWatcher;
        private static ConcurrentQueue<string> m_updatedFiles = new ConcurrentQueue<string>();
        private static HashSet<string> m_updatedFilesInCurrentFrame = new HashSet<string>();

        public static void SetProjectRoot(string p_path)
        {
            if (!string.IsNullOrEmpty(m_projectRoot))
            {
                throw new Exception("Project root already set. It cant be updated");
            }
            m_projectRoot = Path.GetFullPath(p_path);
            m_fileWatcher = new FileWatcher(m_projectRoot, (asset) => m_updatedFiles.Enqueue(asset));
        }

        public static void RegisterLoader<T>(IAssetLoader p_loader) where T : Asset
        {
            var typeName = typeof(T);
            if (m_loaders.ContainsKey(typeName))
            {
                throw new Exception($"Loader already exist for {typeName}");
            }

            m_loaders[typeName] = p_loader;
        }

        public static T Get<T>(string p_identifier) where T : Asset
        {
            if (string.IsNullOrEmpty(p_identifier))
            {
                return null;
            }
            p_identifier = NormalizeIdentifierSeparator(p_identifier);
            if (!m_resources.ContainsKey(p_identifier))
            {
                var path = GetMetadataPath(p_identifier);
                if (!File.Exists(path))
                {
                    UnityEngine.Debug.LogError($"File not found {path}");
                    return null;
                }
                m_resources[p_identifier] = m_loaders[typeof(T)].Load(path);
            }
            return (T)m_resources[p_identifier];
        }

        private static string NormalizeIdentifierSeparator(string p_identifier)
        {
            if (string.IsNullOrEmpty(p_identifier))
            {
                return p_identifier;
            }
            return p_identifier.Replace('\\', '/');
        }

        private static string GetMetadataPath(string p_identifier)
        {
            var fullPath = Path.Combine(m_projectRoot, p_identifier) + ".yaml";
            return fullPath;
        }

        private static string GetIdentifier(string p_fullPath)
        {
            var relativePath = Path.GetRelativePath(m_projectRoot, p_fullPath);
            var indexOfExtention = relativePath.LastIndexOf('.');
            var identifier = relativePath;
            if (indexOfExtention > 0)
            {
                identifier = relativePath.Substring(0, indexOfExtention);
            }
            return NormalizeIdentifierSeparator(identifier);
        }

        public static void Sync()
        {
            m_updatedFilesInCurrentFrame.Clear();
            while (m_updatedFiles.TryDequeue(out var path))
            {
                if (m_updatedFilesInCurrentFrame.Contains(path))
                {
                    continue;
                }
                m_updatedFilesInCurrentFrame.Add(path);
                OnFileUpdated(path);
            }
        }

        private static void OnFileUpdated(string p_path)
        {
            if (!File.Exists(p_path)) // could be a SubDirectory
            {
                return;
            }
            var identifier = GetIdentifier(p_path);
            if (!m_resources.ContainsKey(identifier))
            {
                return;
            }

            var metadataPath = GetMetadataPath(identifier);
            var oldAsset = m_resources[identifier];
            var loader = m_loaders[oldAsset.GetType()];
            var newAsset = loader.Load(metadataPath);
            m_resources[identifier] = newAsset;
            oldAsset.Updated(newAsset);
            oldAsset.Dispose();
        }
    }
}