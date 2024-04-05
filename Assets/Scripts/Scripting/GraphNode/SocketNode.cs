using UnityEngine;

using TMPro;

namespace Cgw.Scripting.GraphNode
{
    public class SocketNode<T> : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_label;
        [SerializeField] private T m_socket;

        public string Label 
        { 
            get => m_label.text; 
            set { m_label.text = value; } 
        }

        public T Socket => m_socket;

        protected void Awake()
        {
            if (m_label != null)
            {
                m_label.raycastTarget = false;
            }
        }
    }
}