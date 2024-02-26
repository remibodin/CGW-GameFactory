using RuntimeNodeEditor;
using System.Text;

namespace Assets.Nodes
{

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