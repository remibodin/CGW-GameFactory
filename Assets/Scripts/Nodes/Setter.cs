using System.Text;

using RuntimeNodeEditor;

namespace Cgw.Scripting.Graph
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
            OnConnectionEvent += Setter_OnConnectionEvent;
        }

        private void Setter_OnConnectionEvent(SocketInput arg1, IOutput arg2)
        {
            var connection = arg1.Connections[0]; // Assumes single connection
            if (connection != null)
            {
                LuaGraphNode prevNode = connection.output.OwnerNode as LuaGraphNode;
                Entrypoint entrypoint = prevNode as Entrypoint;

                if (prevNode != null)
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
}