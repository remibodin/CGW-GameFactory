using RuntimeNodeEditor;
using System.Text;

namespace Assets.Nodes
{

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