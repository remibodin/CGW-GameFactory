using UnityEngine;
using UnityEngine.EventSystems;

using RuntimeNodeEditor;
using Cgw.Scripting.GraphNode;

namespace Cgw.Test
{
    public class GraphNodes : NodeEditor
    {

        public override void StartEditor(NodeGraph graph)
        {
            base.StartEditor(graph);

            Graph.Create("Nodes/EntryPoint", Vector2.zero);

            var addNodeObject = Graph.Create("Nodes/LuaFunction", new Vector2(0, -100));
            var addNode = addNodeObject.GetComponent<LuaFunctionNode>();
            addNode.Data = new LuaFunctionNodeData()
            {
                Name = "Add",
                In = new string[] {"A", "B"},
                Out = true,
                Lua = "{{OUT}} = {{A}} + {{B}}"
            };
            addNode.Gen();

            var jumpNodeObject = Graph.Create("Nodes/LuaFunction", new Vector2(0, -200));
            var jumpNode = jumpNodeObject.GetComponent<LuaFunctionNode>();
            jumpNode.Data = new LuaFunctionNodeData()
            {
                Name = "Jump",
                In = new string[] {"Force"},
                Out = false,
                Lua = "this:Jump({{Force}})"
            };
            jumpNode.Gen();
        }
    }
}

