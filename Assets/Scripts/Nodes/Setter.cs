using RuntimeNodeEditor;
using System.Text;

namespace Assets.Nodes
{
    public class Setter : LuaGraphNode
    {
        public BlackboardEntry BlackboardEntry;

        public SocketInput PinInSocket;

        public override void GenerateLua(StringBuilder output)
        {
            output.Append(string.Format("blackboard[\"{0}\"]", BlackboardEntry.Name));
        }

        public override LuaGraphNode GetPrevNode()
        {
            if (PinInSocket != null)
            {
                var connection = PinInSocket.Connections[0]; // Assuming single connection
                if (connection != null)
                {
                    return connection.input.OwnerNode as LuaGraphNode;
                }
            }
            return null;
        }

        public override void Setup()
        {
            base.Setup();

            Register(PinInSocket);

            headerText.text = BlackboardEntry.Name;
        }
    }
}