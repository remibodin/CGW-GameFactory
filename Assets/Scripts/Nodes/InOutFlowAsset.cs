using Cgw.Assets;
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
}