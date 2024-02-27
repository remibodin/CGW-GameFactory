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
            string template = GetComponent<InOutFlowAssetBehaviour>().Asset.Template;
            output.AppendLine(string.Format(template));
            var nextNode = GetNextNode(FlowOutSocket);
            if (nextNode != null)
            {
                nextNode.GenerateLua(output);
            }
        }

        public override LuaGraphNode GetPrevNode()
        {
            var connection = FlowInSocket.Connections.Count > 0 ? FlowInSocket.Connections[0] : null; // Assumes single connection
            if (connection != null)
            {
                var prevSocket = connection.output;
                if (prevSocket != null)
                {
                    return prevSocket.OwnerNode as LuaGraphNode;
                }
            }
            return null;
        }

        public override void Setup()
        {
            base.Setup();
            Register(FlowInSocket);
            Register(FlowOutSocket);

            OnConnectionEvent += InOutFlowNode_OnConnectionEvent;
        }

        private void InOutFlowNode_OnConnectionEvent(SocketInput arg1, IOutput arg2)
        {
            LuaGraphNode prevNode = GetPrevNode();
            Entrypoint entrypoint = prevNode as Entrypoint;

            while (entrypoint == null)
            {
                prevNode = prevNode.GetPrevNode();
                if (prevNode == null)
                {
                    return;
                }
                entrypoint = prevNode as Entrypoint;
            }

            StringBuilder output = new("");
            entrypoint.GenerateLua(output);
        }
    }
}