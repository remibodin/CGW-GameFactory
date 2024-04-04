using YamlDotNet.Serialization;

using Cgw.Assets;

namespace Cgw.Scripting.Graph
{
    public class NodeCollection : Asset
    {
        [YamlMember(Alias = "entry_point_nodes")]
        public string[] m_EntrypointIdentifiers;

        [YamlMember(Alias = "in_out_flow_nodes")]
        public string[] m_InOutFlowNodeIdentifiers;

        [YamlMember(Alias = "condition_nodes")]
        public string[] m_ConditionIdentifiers;
    }
}