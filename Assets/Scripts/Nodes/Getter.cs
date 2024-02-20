using RuntimeNodeEditor;

namespace Assets.Nodes
{
    public class Getter : Node
    {
        public SocketOutput PinOutSocket;

        public override void Setup()
        {
            Register(PinOutSocket);
        }
    }
}