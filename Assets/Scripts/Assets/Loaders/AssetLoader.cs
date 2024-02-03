using System.IO;

using YamlDotNet.Serialization;

namespace Cgw.Assets.Loaders
{
    public abstract class AssetLoader<T> : IAssetLoader
    where T : Asset
    {
        public Asset Load(string p_path)
        {
            var sData = File.ReadAllText(p_path);
            var deserializer = new Deserializer();
            var data = deserializer.Deserialize<T>(sData);

            return LoadAsset(p_path, data);
        }

        public abstract T LoadAsset(string p_path, T p_data);
    }
}