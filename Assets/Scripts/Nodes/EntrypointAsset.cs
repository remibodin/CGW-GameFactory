using Cgw.Assets;
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
}