using YamlDotNet.Serialization;

using Cgw.Assets;

namespace Cgw.Scripting.Graph
{
    public class InOutFlowAsset : Asset
    {
        [YamlMember(Alias = "name")]
        public string Name;

        [YamlMember(Alias = "description")]
        public string Description;

        [YamlMember(Alias = "params")]
        public string[] Params;

        [YamlMember(Alias = "result")]
        public string Result;

        [YamlMember(Alias = "template")]
        public string Template;
    }
}