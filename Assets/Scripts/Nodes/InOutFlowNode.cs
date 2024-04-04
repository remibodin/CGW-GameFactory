using System.Text;

using TMPro;
using RuntimeNodeEditor;

namespace Cgw.Scripting.Graph
{

    public class InOutFlowNode : LuaGraphNode
    {
        public string[] Params;
        public string Result;

        public SocketInput FlowInSocket;
        public SocketOutput FlowOutSocket;

        public SocketInput ParamSocket1;
        public TMP_Text ParamSocket1Label;
        public SocketInput ParamSocket2;
        public TMP_Text ParamSocket2Label;

        public SocketOutput ResultSocket;
        public TMP_Text ResultSocketLabel;

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

            // TODO on peut sûrement faire mieux
            if (Params != null)
            {
                switch (Params.Length)
                {
                    case 0:
                        ParamSocket1.gameObject.SetActive(false);
                        ParamSocket2.gameObject.SetActive(false);
                        break;
                    case 1:
                        Register(ParamSocket1);
                        ParamSocket1Label.text = Params[0];
                        ParamSocket2.gameObject.SetActive(false);
                        break;
                    default:
                        Register(ParamSocket1);
                        ParamSocket1.gameObject.SetActive(true);
                        ParamSocket1Label.text = Params[0];
                        Register(ParamSocket2);
                        ParamSocket2.gameObject.SetActive(true);
                        ParamSocket2Label.text = Params[1];
                        break;
                }
            }

            if (Result != null)
            {
                Register(ResultSocket);
                ResultSocket.gameObject.SetActive(true);
                ResultSocketLabel.text = Result;
            }
            else
            {
                ResultSocket.gameObject.SetActive(false);
            }

            OnConnectionEvent += InOutFlowNode_OnConnectionEvent;
        }

        private void InOutFlowNode_OnConnectionEvent(SocketInput arg1, IOutput arg2)
        {
            LuaGraphNode prevNode = GetPrevNode();
            Entrypoint entrypoint = prevNode as Entrypoint;

            if (prevNode  != null)
            {
                while (entrypoint == null)
                {
                    prevNode = prevNode.GetPrevNode();
                    if (prevNode == null)
                    {
                        return;
                    }
                    entrypoint = prevNode as Entrypoint;
                }
                entrypoint.NotifyLoader();
            }
        }
    }
}