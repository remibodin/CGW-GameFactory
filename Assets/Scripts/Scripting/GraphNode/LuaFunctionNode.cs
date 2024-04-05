using UnityEngine;
using UnityEngine.UI;

using RuntimeNodeEditor;

namespace Cgw.Scripting.GraphNode
{
    public interface ILuaConvertable
    {
        string LuaString();
    }

    [System.Serializable]
    public class LuaFunctionNodeData
    {
        // Must be unique
        public string Name;
        public string[] In;
        public bool Out;
        public string Lua;
    }

    public class LuaFunctionNode : Node, ILuaConvertable
    {
        [Header("Ref")]
        [SerializeField] private SocketInput m_flowInSocket;
        [SerializeField] private SocketOutput m_flowOutSocket;
        [SerializeField] private LayoutGroup m_inContainer;
        [SerializeField] private LayoutGroup m_outContainer;

        [Header("Assets")]
        [SerializeField] private NodeParameter m_nodeParameterPrefab;
        [SerializeField] private NodeReturn m_nodeReturnPrefab;

        [Header("DEBUG")]
        public LuaFunctionNodeData Data;

        public override void Setup()
        {

            base.Setup();
            Register(m_flowInSocket);
            Register(m_flowOutSocket);
        }

        public void Gen()
        {
            headerText.text = Data.Name;
            foreach (var inputParam in Data.In)
            {
                var nodeParameter = Instantiate(m_nodeParameterPrefab, m_inContainer.transform);
                nodeParameter.Label = inputParam;
                Register(nodeParameter.Socket);
            }
            if (Data.Out)
            {
                var nodeReturn = Instantiate(m_nodeReturnPrefab, m_outContainer.transform);
                nodeReturn.Label = "Result";
                Register(nodeReturn.Socket);
            }
            var rectTransform = GetComponent<RectTransform>();
            var sizeDelta = rectTransform.sizeDelta;
            sizeDelta.y = 42 + (42 * Data.In.Length);
            rectTransform.sizeDelta = sizeDelta;
        }

        public string LuaString()
        {
            var lua = string.Empty;
            foreach (var inputParam in Data.In)
            {
                lua = lua.Replace(inputParam, "");
            }
            if (Data.Out)
            {
                lua = $"ret = {lua}"; // TODO: replace 'ret' by the output blackbord var name
            }
            return lua;
        }
    }
}