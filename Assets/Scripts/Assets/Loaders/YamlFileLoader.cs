namespace Cgw.Assets.Loaders
{
    public class YamlFileLoader<T> : AssetLoader<T>
    where T : Asset, new()
    {
        public override T LoadAsset(string p_path, T p_data)
        {
            return p_data;
        }
    }
}