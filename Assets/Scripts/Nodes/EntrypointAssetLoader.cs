using Cgw.Assets.Loaders;

namespace Assets.Nodes
{
    public class EntrypointAssetLoader : AssetLoader<EntrypointAsset>
    {
        public override EntrypointAsset LoadAsset(string p_path, EntrypointAsset p_data)
        {
            return p_data;
        }
    }
}