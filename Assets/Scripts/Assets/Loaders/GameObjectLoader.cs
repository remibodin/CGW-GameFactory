using System.Collections.Generic;

using UnityEngine;

namespace Cgw.Assets.Loaders
{
    public class GameObjectLoader : AssetFileLoader<GameObjectAsset>
    {
        private string[] m_extentions = new string[] { "asset" };

        public override IEnumerable<string> Extentions => m_extentions;

        public override GameObjectAsset LoadAsset(string p_metadataPath, string p_filePath, GameObjectAsset p_data)
        {
            var bundle = AssetBundle.LoadFromFile(p_filePath);
            if (bundle == null)
            {
                Debug.Log($"Failed to load asset {p_filePath}");
                return p_data;
            }
            p_data.GameObject = bundle.LoadAsset<GameObject>(p_data.AssetName);

            bundle.Unload(false);

            return p_data;
        }
    }
}