using System.Collections.Generic;
using System.IO;

namespace Cgw.Assets.Loaders
{
    public abstract class AssetFileLoader<T> : AssetLoader<T>
    where T : Asset, new()
    {
        public virtual IEnumerable<string> Extentions { get; }

        public override T LoadAsset(string p_path, T p_data)
        {
            if (Extentions == null)
            {
                throw new System.Exception("Extentions cant be null");
            }
            foreach (var ext in Extentions)
            {
                var filePath = Path.ChangeExtension(p_path, ext);
                if (File.Exists(filePath))
                {
                    return LoadAsset(p_path, filePath, p_data);
                }
            }
            return LoadAsset(p_path, null, p_data);
        }

        public abstract T LoadAsset(string p_metadataPath, string p_filePath, T p_data);
    }
}