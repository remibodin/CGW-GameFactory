using RuntimeNodeEditor;
using System.Text;

namespace Assets.Nodes
{

    public class Entrypoint : LuaGraphNode
    {
        public SocketOutput FlowOutSocket;

        public override void GenerateLua(StringBuilder output)
        {
            throw new System.NotImplementedException();
        }

        public override void Setup()
        {
            Register(FlowOutSocket);
        }
    }
}