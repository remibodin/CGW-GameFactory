using Cgw.Assets.Loaders;

namespace Cgw.Scripting.Graph
{
    public class EntrypointAssetLoader : AssetLoader<EntrypointAsset>
    {
        public override EntrypointAsset LoadAsset(string p_path, EntrypointAsset p_data)
        {
            return p_data;
        }
    }
}