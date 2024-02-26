using Cgw.Assets;
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
}