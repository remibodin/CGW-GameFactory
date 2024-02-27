using RuntimeNodeEditor;
using System;
using System.Text;
using UnityEngine;

namespace Assets.Nodes
{

    public class Entrypoint : LuaGraphNode
    {
        public SocketOutput FlowOutSocket;

        public override void GenerateLua(StringBuilder output)
        {
            string template = GetComponent<EntrypointAssetBehaviour>().Asset.Template;
            StringBuilder corpus = new("");

            var nextNode = GetNextNode(FlowOutSocket);
            if (nextNode != null)
            {
                nextNode.GenerateLua(corpus);
            }

            output.AppendLine(string.Format(template, corpus));
            Debug.Log(output.ToString());
        }

        public override LuaGraphNode GetPrevNode()
        {
            return null;
        }

        public override void Setup()
        {
            base.Setup();
            Register(FlowOutSocket);

            OnConnectionEvent += Entrypoint_OnConnectionEvent;
        }

        private void Entrypoint_OnConnectionEvent(SocketInput arg1, IOutput arg2)
        {
            StringBuilder output = new("");
            GenerateLua(output);
        }
    }
}