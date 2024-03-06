using RuntimeNodeEditor;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Nodes
{
    public class NodeGraphCompiler : MonoBehaviour
    {
        private Blackboard m_Blackboard;
        private NodeLoader m_Nodes;

        private void Awake()
        {
            m_Blackboard = GetComponent<Blackboard>();
            m_Nodes = GetComponent<NodeLoader>();
            m_Nodes.OnNodesUpdated += Nodes_OnNodesUpdated;
        }

        private void Nodes_OnNodesUpdated(IEnumerable<Entrypoint> entrypoints)
        {
            GenerateScript(entrypoints);
        }

        public void GenerateScript(IEnumerable<Entrypoint> entrypoints)
        {
            StringBuilder output = new("");
            m_Blackboard.GenerateBlackboardHeader(output);
            foreach (var entrypoint in entrypoints)
            {
                entrypoint.GenerateLua(output);
                output.AppendLine("");
            }
            Debug.Log(output.ToString());
        }
    }
}