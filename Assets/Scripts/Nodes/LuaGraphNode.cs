using Cgw.Assets;
using RuntimeNodeEditor;
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

    }
}