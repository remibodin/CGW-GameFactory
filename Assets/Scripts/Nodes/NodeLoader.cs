using Cgw;
using Cgw.Assets;
using Cgw.Test;
using RuntimeNodeEditor;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Nodes
{
    public class NodeLoader : AssetBehaviour<NodeCollection>
    {
        public NodeCollection NodeCollection;

        public Dictionary<string, EntrypointAssetBehaviour> EntrypointNodes = new();
        public Dictionary<string, ConditionAssetBehaviour> ConditionNodes = new();
        public Dictionary<string, InOutFlowAssetBehaviour> InOutFlowNodes = new();

        public string EntrypointPrefabPath;
        public string ConditionPrefabPath;
        public string InOutFlowPrefabPath;

        private GameObject m_GraphHolder;
        private NodeGraph m_NodeGraph;
        private static NodeLoader m_Instance;

        private void Start()
        {
            m_Instance = this;
            m_NodeGraph = GetComponent<GraphNodes>().Graph;
            NodeCollection = ResourcesManager.Get<NodeCollection>("Nodes/node_collection");
        }

        protected override void AssetUpdated()
        {
            UpdateNodes();
        }

        private void UpdateNodes()
        {
            m_NodeGraph = GetComponent<GraphNodes>().Graph;
            UpdateEntrypoints();
            UpdateConditions();
            UpdateInOutFlows();

        }

        [TermCommand]
        public static void ReloadNodes(string p_args)
        {
            m_Instance.UpdateNodes();
        }

        private void UpdateEntrypoints()
        {
            foreach (string identifier in NodeCollection.m_EntrypointIdentifiers)
            {
                if (!EntrypointNodes.ContainsKey(identifier))
                {
                    GameObject newEntrypoint = m_NodeGraph.Create(EntrypointPrefabPath, Vector2.zero);
                    EntrypointNodes[identifier] = newEntrypoint.GetComponent<EntrypointAssetBehaviour>();
                }

                EntrypointAssetBehaviour entrypoint = EntrypointNodes[identifier];
                entrypoint.EntrypointAsset = ResourcesManager.Get<EntrypointAsset>("Nodes/Entrypoint/" + identifier);
            }

            foreach (var pair in  EntrypointNodes)
            {
                if (!NodeCollection.m_EntrypointIdentifiers.Contains(pair.Key))
                {
                    DestroyImmediate(pair.Value.gameObject);
                }
            }
        }

        private void UpdateConditions()
        {
            foreach (string identifier in NodeCollection.m_ConditionIdentifiers)
            {
                if (!ConditionNodes.ContainsKey(identifier))
                {
                    GameObject newCondition = m_NodeGraph.Create(ConditionPrefabPath, Vector2.zero);
                    ConditionNodes[identifier] = newCondition.GetComponent<ConditionAssetBehaviour>();
                }

                ConditionAssetBehaviour condition = ConditionNodes[identifier];
                condition.ConditionAsset = ResourcesManager.Get<ConditionAsset>("Nodes/Condition/" + identifier);
            }

            foreach (var pair in ConditionNodes)
            {
                if (!NodeCollection.m_ConditionIdentifiers.Contains(pair.Key))
                {
                    DestroyImmediate(pair.Value.gameObject);
                }
            }
        }

        private void UpdateInOutFlows()
        {
            foreach (string identifier in NodeCollection.m_InOutFlowNodeIdentifiers)
            {
                if (!InOutFlowNodes.ContainsKey(identifier))
                {
                    GameObject newInOutFlow = m_NodeGraph.Create(InOutFlowPrefabPath, Vector2.zero);
                    InOutFlowNodes[identifier] = newInOutFlow.GetComponent<InOutFlowAssetBehaviour>();
                }

                InOutFlowAssetBehaviour inOutFlow = InOutFlowNodes[identifier];
                inOutFlow.InOutFlowAsset = ResourcesManager.Get<InOutFlowAsset>("Nodes/InOutFlowNodes/" + identifier);
            }

            foreach (var pair in InOutFlowNodes)
            {
                if (!NodeCollection.m_InOutFlowNodeIdentifiers.Contains(pair.Key))
                {
                    DestroyImmediate(pair.Value.gameObject);
                }
            }
        }
    }
}