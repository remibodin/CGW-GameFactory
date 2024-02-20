using UnityEngine;

using RuntimeNodeEditor;

namespace Cgw.Test
{
    public class GraphNodesCreator : MonoBehaviour
    {
        public GraphNodes Graph;
        public RectTransform Holder;
        public Color BackgroundColor;
        public Color ConnectionColor;

        public void Start()
        {
            var editor = Graph.CreateGraph<NodeGraph>(Holder, BackgroundColor, ConnectionColor);
            Graph.StartEditor(editor);
        }

    }
}

