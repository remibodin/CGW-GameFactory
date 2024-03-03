using Cgw.Assets;

namespace Assets.Nodes
{
    public class InOutFlowAssetBehaviour : AssetBehaviour<InOutFlowAsset>
    {
        public InOutFlowAsset InOutFlowAsset
        {
            get { return Asset; }
            set { Asset = value; }
        }

        protected override void AssetUpdated()
        {
            var inOutFlowNode = GetComponent<InOutFlowNode>();
            inOutFlowNode.name = InOutFlowAsset.Name;
            inOutFlowNode.SetHeader(InOutFlowAsset.Name);
            inOutFlowNode.Template = InOutFlowAsset.Template;
            inOutFlowNode.Params = InOutFlowAsset.Params;
            inOutFlowNode.Result = InOutFlowAsset.Result;
        }
    }
}