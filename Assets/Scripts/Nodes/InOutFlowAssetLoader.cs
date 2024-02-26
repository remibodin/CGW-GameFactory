using Cgw.Assets.Loaders;

namespace Assets.Nodes
{
    public class InOutFlowAssetLoader : AssetLoader<InOutFlowAsset>
    {
        public override InOutFlowAsset LoadAsset(string p_path, InOutFlowAsset p_data)
        {
            return p_data;
        }
    }
}