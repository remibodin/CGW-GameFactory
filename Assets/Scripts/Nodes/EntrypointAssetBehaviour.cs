using Cgw.Assets;

namespace Cgw.Scripting.Graph
{
    public class EntrypointAssetBehaviour : AssetBehaviour<EntrypointAsset>
    {
        public EntrypointAsset EntrypointAsset
        {
            get { return Asset; }
            set { Asset = value; }
        }

        protected override void AssetUpdated()
        {
            var entrypoint = GetComponent<Entrypoint>();
            entrypoint.name = EntrypointAsset.Name;
            entrypoint.SetHeader(EntrypointAsset.Name);
            entrypoint.Template = EntrypointAsset.Template;
            entrypoint.Setup();
        }
    }
}