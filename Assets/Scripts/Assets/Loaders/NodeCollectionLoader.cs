using Assets.Nodes;

namespace Cgw.Assets.Loaders
{
    public class NodeCollectionLoader : AssetLoader<NodeCollection>
    {
        public override NodeCollection LoadAsset(string p_path, NodeCollection p_data)
        {
            return p_data;
        }
    }
}