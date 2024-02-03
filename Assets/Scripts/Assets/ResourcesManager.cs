using System;
using System.Collections.Generic;
using System.IO;

using Cgw.Assets.Loaders;

namespace Cgw.Assets
{
    public class ResourcesManager
    {
        private static Dictionary<Type, IAssetLoader> m_loaders = new Dictionary<Type, IAssetLoader>();
        private static Dictionary<string, Asset> m_resources = new Dictionary<string, Asset>();
        private static string m_projectRoot;

        public static void SetProjectRoot(string p_path)
        {
            m_projectRoot = Path.GetFullPath(p_path);
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
            if (!m_resources.ContainsKey(p_identifier))
            {
                var path = GetFullPath(p_identifier);
                m_resources[p_identifier] = m_loaders[typeof(T)].Load(path);
            }
            return (T)m_resources[p_identifier];
        }

        private static string GetFullPath(string p_identifier)
        {
            var fullPath = Path.Combine(m_projectRoot, p_identifier) + ".yaml";
            return fullPath;
        }
    }
}