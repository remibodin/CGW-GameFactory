using Cgw.Assets;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Nodes
{
    public class NodeLoader : AssetBehaviour<NodeCollection>
    {
        public NodeCollection NodeCollection;

        public Dictionary<string, EntrypointAssetBehaviour> EntrypointNodes = new();
        public Dictionary<string, ConditionAssetBehaviour> ConditionNodes = new();
        public Dictionary<string, InOutFlowAssetBehaviour> InOutFlowNodes = new();

        public GameObject EntrypointPrefab;
        public GameObject ConditionPrefab;
        public GameObject InOutFlowPrefab;

        public GameObject GraphHolder;

        public void Awake()
        {
            NodeCollection = ResourcesManager.Get<NodeCollection>("Nodes/node_collection");
            UpdateNodes();
        }

        protected override void AssetUpdated()
        {
            UpdateNodes();
        }

        private void UpdateNodes()
        {
            UpdateEntrypoints();
            UpdateConditions();
            UpdateInOutFlows();
        }

        private void UpdateEntrypoints()
        {
            foreach (string identifier in NodeCollection.m_EntrypointIdentifiers)
            {
                if (!EntrypointNodes.ContainsKey(identifier))
                {
                    GameObject newEntrypoint = Instantiate(EntrypointPrefab, Vector3.zero, Quaternion.identity);
                    newEntrypoint.transform.SetParent(GraphHolder.transform);
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
                    GameObject newCondition = Instantiate(ConditionPrefab, Vector3.zero, Quaternion.identity);
                    newCondition.transform.SetParent(GraphHolder.transform);
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
                    GameObject newInOutFlow = Instantiate(InOutFlowPrefab, Vector3.zero, Quaternion.identity);
                    newInOutFlow.transform.SetParent(GraphHolder.transform);
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