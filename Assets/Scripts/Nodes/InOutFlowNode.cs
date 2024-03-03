using RuntimeNodeEditor;
using System.Text;

namespace Assets.Nodes
{

    public class InOutFlowNode : LuaGraphNode
    {
        public string[] Params;
        public string Result;

        public SocketInput FlowInSocket;
        public SocketOutput FlowOutSocket;

        public SocketInput ParamSocket1;
        public SocketInput ParamSocket2;

        public SocketOutput ResultSocket;

        public override void GenerateLua(StringBuilder output)
        {
            string template = GetComponent<InOutFlowAssetBehaviour>().Asset.Template;
            string param1 = "";
            string param2 = "";
            string result = "";

            GetParam(ParamSocket1, ref param1);
            GetParam(ParamSocket2, ref param2);
            GetResult(ResultSocket, ref result);

            output.AppendLine(string.Format(template, result, param1, param2));
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

            Register(ParamSocket1);
            Register(ParamSocket2);

            Register(ResultSocket);

            // TODO on peut sûrement faire mieux
            switch (Params.Length)
            {
                case 0:
                    ParamSocket1.enabled = false;
                    ParamSocket2.enabled = false;
                    break;
                case 1:
                    ParamSocket2.enabled = false;
                    break;
                default:
                    break;
            }

            if (Result == null)
            {
                ResultSocket.enabled = false;
            }

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