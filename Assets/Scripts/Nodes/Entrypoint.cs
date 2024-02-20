using Cgw.Assets;
using Cgw.Assets.Loaders;
using RuntimeNodeEditor;
using System.Text;
using YamlDotNet.Serialization;

namespace Assets.Nodes
{
    public class EntrypointAsset : Asset
    {
        [YamlMember(Alias = "name")]
        public string Name;

        [YamlMember(Alias = "description")]
        public string Description;

        [YamlMember(Alias = "template")]
        public string Template;
    }

    public class EntrypointAssetLoader : AssetLoader<EntrypointAsset>
    {
        public override EntrypointAsset LoadAsset(string p_path, EntrypointAsset p_data)
        {
            return p_data;
        }
    }

    public class Entrypoint : LuaGraphNode
    {
        public SocketOutput FlowOutSocket;

        public override void GenerateLua(StringBuilder output)
        {
            throw new System.NotImplementedException();
        }

        public override void Setup()
        {
            Register(FlowOutSocket);
        }
    }
}