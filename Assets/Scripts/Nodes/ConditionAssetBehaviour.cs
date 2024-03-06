using Cgw.Assets;
using UnityEngine.Rendering;

namespace Assets.Nodes
{
    public class ConditionAssetBehaviour : AssetBehaviour<ConditionAsset>
    {
        public ConditionAsset ConditionAsset
        {
            get { return Asset; }
            set { Asset = value; }
        }

        protected override void AssetUpdated()
        {
            var condition = GetComponent<Condition>();
            condition.SetHeader(ConditionAsset.Name);
            condition.name = ConditionAsset.Name;
            condition.Template = ConditionAsset.Template;
            condition.Params = ConditionAsset.Params;
            condition.Setup();
        }
    }
}