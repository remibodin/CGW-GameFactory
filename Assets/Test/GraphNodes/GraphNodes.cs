using UnityEngine;
using UnityEngine.EventSystems;

using RuntimeNodeEditor;

namespace Cgw.Test
{
    public class GraphNodes : NodeEditor
    {

        public override void StartEditor(NodeGraph graph)
        {
            base.StartEditor(graph);

            Graph.Create("Nodes/EntryPoint", Vector2.zero);
            Graph.Create("Nodes/Condition", new Vector2(0, -150));
        }
    }
}

