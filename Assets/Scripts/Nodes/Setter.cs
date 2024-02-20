using RuntimeNodeEditor;

namespace Assets.Nodes
{

    public class Setter : Node
    {
        public SocketInput PinInSocket;

        public override void Setup()
        {
            Register(PinInSocket);
        }
    }
}