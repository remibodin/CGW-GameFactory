using Cgw.Assets;
using Cgw.Assets.Loaders;
using RuntimeNodeEditor;
using System.Text;
using YamlDotNet.Serialization;

namespace Assets.Nodes
{
    public class InOutFlowAsset : Asset
    {
        [YamlMember(Alias = "name")]
        public string Name;

        [YamlMember(Alias = "description")]
        public string Description;

        [YamlMember(Alias = "params")]
        public string[] Params;

        [YamlMember(Alias = "return")]
        public string Return;

        [YamlMember(Alias = "template")]
        public string Template;
    }

    public class InOutFlowAssetLoader : AssetLoader<InOutFlowAsset>
    {
        public override InOutFlowAsset LoadAsset(string p_path, InOutFlowAsset p_data)
        {
            return p_data;
        }
    }

    public class InOutFlowNode : LuaGraphNode
    {
        public string[] Params;

        public SocketInput FlowInSocket;
        public SocketOutput FlowOutSocket;

        public override void GenerateLua(StringBuilder output)
        {
            throw new System.NotImplementedException();
        }

        public override void Setup()
        {
            Register(FlowInSocket);
            Register(FlowOutSocket);
        }
    }
}