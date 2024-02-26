using Cgw.Assets.Loaders;

namespace Assets.Nodes
{
    public class ConditionAssetLoader : AssetLoader<ConditionAsset>
    {
        public override ConditionAsset LoadAsset(string p_path, ConditionAsset p_data)
        {
            return p_data;
        }
    }
}