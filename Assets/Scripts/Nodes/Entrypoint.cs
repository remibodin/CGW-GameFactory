using RuntimeNodeEditor;
using System;
using System.Text;
using UnityEngine;

namespace Assets.Nodes
{

    public class Entrypoint : LuaGraphNode
    {
        public SocketOutput FlowOutSocket;
        public NodeLoader Loader;

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

        public void NotifyLoader()
        {
            if (Loader != null)
            {
                Loader.UpdateNodes();
            }
        }

        private void Entrypoint_OnConnectionEvent(SocketInput arg1, IOutput arg2)
        {
            NotifyLoader();
        }
    }
}