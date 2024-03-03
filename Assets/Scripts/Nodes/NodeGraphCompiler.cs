using RuntimeNodeEditor;
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
        }

        public void GenerateScript()
        {
            StringBuilder output = new("");
            m_Blackboard.GenerateBlackboardHeader(output);
            m_Nodes.GenerateMethodes(output);
            Debug.Log(output.ToString());
        }
    }
}