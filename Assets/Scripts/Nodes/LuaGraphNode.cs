using Cgw.Assets;
using RuntimeNodeEditor;
using System.Diagnostics;
using System.Text;

namespace Assets.Nodes
{
    public abstract class LuaGraphNode : Node
    {
        public string Template;

        public abstract void GenerateLua(StringBuilder output);
        public abstract LuaGraphNode GetPrevNode();

        public LuaGraphNode GetNextNode(SocketOutput socketOutput)
        {
            var connection = socketOutput.connection;
            if (connection != null)
            {
                var nextSocket = connection.input;
                if (nextSocket != null)
                {
                    return nextSocket.OwnerNode as LuaGraphNode;
                }
            }
            return null;
        }

        public void GetParam(SocketInput socketInput, ref string value)
        {
            var connections = socketInput.Connections;
            if (connections != null && connections.Count > 0)
            {
                var connection = connections[0]; // assumes single connection
                if (connection != null)
                {
                    var prevSocket = connection.output;
                    if (prevSocket != null)
                    {
                        var getter = prevSocket.OwnerNode.GetComponent<Getter>();
                        if (getter != null)
                        {
                            StringBuilder output = new();
                            getter.GenerateLua(output);
                            value = output.ToString();
                        }
                    }
                }
            }
        }

        public void GetResult(SocketOutput socketOutput, ref string result)
        {
            var connection = socketOutput.connection;
            if (connection != null)
            {
                var nextSocket = connection.input;
                if (nextSocket != null)
                {
                    var setter = nextSocket.OwnerNode?.GetComponent<Setter>();
                    if (setter != null)
                    {
                        StringBuilder output = new();
                        setter.GenerateLua(output);
                        result = output.ToString();
                    }
                }
            }
        }
    }
}