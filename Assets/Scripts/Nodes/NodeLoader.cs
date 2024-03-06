using Cgw;
using Cgw.Assets;
using Cgw.Test;
using RuntimeNodeEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public Dictionary<string, Getter> GetterNodes = new();
        public Dictionary<string, Setter> SetterNodes = new();

        public string EntrypointPrefabPath;
        public string ConditionPrefabPath;
        public string InOutFlowPrefabPath;
        public string GetterPrefabPath;
        public string SetterPrefabPath;

        private NodeGraph m_NodeGraph;
        private static NodeLoader m_Instance;

        public event Action<IEnumerable<Entrypoint>> OnNodesUpdated;

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

        public void GenerateMethods(StringBuilder output)
        {
            foreach (var entrypoint in EntrypointNodes)
            {
                var entry = entrypoint.Value.GetComponent<Entrypoint>();
                entry.GenerateLua(output);
                output.AppendLine("");
            }
        }

        public void UpdateNodes()
        {
            m_NodeGraph = GetComponent<GraphNodes>().Graph;
            UpdateEntrypoints();
            UpdateConditions();
            UpdateInOutFlows();
            UpdateBlackboardNodes();
            OnNodesUpdated(EntrypointNodes.Values.Select(x => x.gameObject.GetComponent<Entrypoint>()));
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
                    newEntrypoint.GetComponent<Entrypoint>().Loader = this;
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

        private void UpdateBlackboardNodes()
        {
            var blackboard = GetComponent<Blackboard>();
            if (blackboard != null)
            {
                foreach (var entry in blackboard.Entries)
                {
                    if (!GetterNodes.ContainsKey(entry.Name))
                    {
                        GameObject newGetter = m_NodeGraph.Create(GetterPrefabPath, Vector2.zero);
                        var getter = newGetter.GetComponent<Getter>();
                        getter.headerText.text = entry.Name;
                        getter.BlackboardEntry = entry;
                        GetterNodes[entry.Name] = getter;
                    }

                    if (!SetterNodes.ContainsKey(entry.Name))
                    {
                        GameObject newSetter = m_NodeGraph.Create(SetterPrefabPath, Vector2.zero);
                        var setter = newSetter.GetComponent<Setter>();
                        setter.headerText.text = entry.Name;
                        setter.BlackboardEntry = entry;
                        SetterNodes[entry.Name] = setter;
                    }
                }

                foreach (var pair in GetterNodes)
                {
                    if (!blackboard.Entries.Any(entry => entry.Name == pair.Key))
                    {
                        DestroyImmediate(pair.Value.gameObject);
                    }
                }

                foreach (var pair in SetterNodes)
                {
                    if (!blackboard.Entries.Any(entry => entry.Name == pair.Key))
                    {
                        DestroyImmediate(pair.Value.gameObject);
                    }
                }
            }
        }
    }
}