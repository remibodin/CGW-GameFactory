using RuntimeNodeEditor;
using System.Text;

namespace Assets.Nodes
{
    public class Getter : LuaGraphNode
    {
        public BlackboardEntry BlackboardEntry;

        public SocketOutput PinOutSocket;

        public override void GenerateLua(StringBuilder output)
        {
            output.Append(string.Format("blackboard[\"{0}\"]", BlackboardEntry.Name));
        }

        public override LuaGraphNode GetPrevNode()
        {
            return null;
        }

        public override void Setup()
        {
            base.Setup();

            Register(PinOutSocket);

            headerText.text = BlackboardEntry.Name;
        }
    }
}