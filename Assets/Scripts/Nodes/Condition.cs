using RuntimeNodeEditor;
using System.Text;

namespace Assets.Nodes
{

    public class Condition : LuaGraphNode
    {
        public string[] Params;

        public SocketInput FlowInSocket;

        public SocketOutput FlowOutThenSocket;
        public SocketOutput FlowOutElseSocket;

        public override void GenerateLua(StringBuilder output)
        {
            string template = GetComponent<ConditionAssetBehaviour>().Asset.Template;
            StringBuilder corpusThen = new("");
            StringBuilder corpusElse = new("");

            var nextNodeThen = GetNextNode(FlowOutThenSocket);
            if (nextNodeThen != null)
            {
                nextNodeThen.GenerateLua(corpusThen);
            }

            var nextNodeElse = GetNextNode(FlowOutElseSocket);
            if (nextNodeElse != null)
            {
                nextNodeElse.GenerateLua(corpusElse);
            }

            output.AppendLine(string.Format(template, corpusThen, corpusElse));
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
            Register(FlowOutThenSocket);
            Register(FlowOutElseSocket);

            OnConnectionEvent += Condition_OnConnectionEvent;
        }

        private void Condition_OnConnectionEvent(SocketInput arg1, IOutput arg2)
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