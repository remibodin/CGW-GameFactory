using UnityEngine;

using TMPro;
using UnityEngine.EventSystems;

namespace Cgw
{
    public class Terminal : MonoBehaviour
    {
        [SerializeField] private Canvas m_canvas;
        [SerializeField] private TMP_InputField m_input;
        [SerializeField] private TextMeshProUGUI m_output;

        [Header("Text format")]
        [SerializeField] private Color m_defaultColor;
        [SerializeField] private Color m_warningColor;
        [SerializeField] private Color m_errorColor;

        private void Awake()
        {
            Application.logMessageReceivedThreaded += HandleLog;
        }

        private void Start()
        {
            m_canvas.enabled = false;
        }

        private void OnDestroy()
        {
            Application.logMessageReceivedThreaded -= HandleLog;
        }

        private void HandleLog(string p_logString, string stackTrace, LogType p_type)
        {
            m_output.text += FormatLogString(p_logString, p_type);
        }

        private string FormatLogString(string logString, LogType type) 
        {
            Color color = m_defaultColor;
            switch(type)
            {
                case LogType.Exception:
                case LogType.Assert:
                case LogType.Error: 
                    color = m_errorColor;
                    break;
                case LogType.Warning: color = m_warningColor; break;
            }
            return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{logString}</color>\n";
        }

        public void DisplayTerminal(bool p_value = true)
        {
            if (m_canvas.enabled == p_value)
            {
                return;
            }
            m_canvas.enabled = p_value;
            if (m_canvas.enabled)
            {
                EventSystem.current.SetSelectedGameObject(m_input.gameObject);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F12))
            {
                DisplayTerminal(!m_canvas.enabled);
            }
        }
    }
}
