using Cgw.Assets;
using Cgw.Assets.Loaders;
using RuntimeNodeEditor;
using System.Text;
using YamlDotNet.Serialization;

namespace Assets.Nodes
{
    public class ConditionAsset : Asset
    {
        [YamlMember(Alias = "name")]
        public string Name;

        [YamlMember(Alias = "description")]
        public string Description;

        [YamlMember(Alias = "params")]
        public string[] Params;

        [YamlMember(Alias = "template")]
        public string Template;
    }

    public class ConditionAssetLoader : AssetLoader<ConditionAsset>
    {
        public override ConditionAsset LoadAsset(string p_path, ConditionAsset p_data)
        {
            return p_data;
        }
    }

    public class Condition : InOutFlowNode
    {
        public SocketInput PinInSocket;

        public SocketOutput FlowOutThenSocket;
        public SocketOutput FlowOutElseSocket;

        public override void GenerateLua(StringBuilder output)
        {
            throw new System.NotImplementedException();
        }

        public override void Setup()
        {
            Register(PinInSocket);
            Register(FlowOutThenSocket);
            Register(FlowOutElseSocket);
        }
    }
}