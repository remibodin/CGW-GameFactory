using Cgw.Assets;
using RuntimeNodeEditor;
using System.Text;

namespace Assets.Nodes
{
    public abstract class LuaGraphNode : Node
    {
        public string Template;

        public abstract void GenerateLua(StringBuilder output);
    }
}