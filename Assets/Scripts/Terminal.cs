using System;
using System.Reflection;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

using TMPro;

namespace Cgw
{
    public class Terminal : SingleBehaviour<Terminal>
    {
        [SerializeField] private Canvas m_canvas;
        [SerializeField] private TMP_InputField m_input;
        [SerializeField] private TextMeshProUGUI m_output;

        [Header("Text format")]
        [SerializeField] private Color m_defaultColor;
        [SerializeField] private Color m_warningColor;
        [SerializeField] private Color m_errorColor;

        private Dictionary<string, Action<string>> m_commands = new Dictionary<string, Action<string>>();

        public IEnumerable<string> CommandsName => m_commands.Keys;
        private GameObject m_lastSelectedUIObject;

        protected override void Awake()
        {
            base.Awake();
            Application.logMessageReceivedThreaded += HandleLog;
            RegisterCommands();
        }

        private void Start()
        {
            m_input.onSubmit.AddListener(InputTextField_OnSubmit);
            m_canvas.enabled = false;
        }

        private void OnDestroy()
        {
            Application.logMessageReceivedThreaded -= HandleLog;
        }

        private void HandleLog(string p_logString, string p_stackTrace, LogType p_type)
        {
            if (p_type == LogType.Error)
            {
                p_logString += "\n" + p_stackTrace;
            }
            m_output.text += FormatLogString(p_logString, p_type);
        }

        private void InputTextField_OnSubmit(string p_value)
        {
            p_value = p_value.Trim();

            var index = p_value.IndexOf(" ");
            var commandName = index <= 0 ? p_value : p_value.Substring(0, index);
            commandName = commandName.ToLower();

            string commandArgs = null;
            if (index > 0)
            {
                commandArgs = p_value.Substring(index).Trim();
            }

            m_input.text = string.Empty;
            m_input.ActivateInputField();
            m_input.Select();

            if (m_commands.TryGetValue(commandName, out var action))
            {
                action.Invoke(commandArgs);
            }
            else
            {
                Debug.LogError($"<b>{commandName}</b> Command not found");
            }
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

        private void RegisterCommands()
        {
            var types = Assembly
                .GetExecutingAssembly()
                .GetTypes();

            foreach(var type in types)
            {
                var staticMethods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                foreach(var staticMethod in staticMethods)
                {
                    if (Attribute.GetCustomAttribute(staticMethod, typeof(TermCommandAttribute)) == null)
                    {
                        continue;
                    }
                    var action = (Action<string>)Delegate.CreateDelegate(typeof(Action<string>), staticMethod);
                    m_commands[staticMethod.Name.ToLower()] = action;
                }
            }
        }

        public void DisplayTerminal(bool p_value = true)
        {
            if (m_canvas.enabled == p_value)
            {
                return;
            }
            m_canvas.enabled = p_value;
            if (EventSystem.current == null)
            {
                var eventSystemObject = new GameObject("EventSystem");
                eventSystemObject.AddComponent<StandaloneInputModule>(); // EventSystem component is auto add by StandaloneInputModule
            }
            if (m_canvas.enabled && EventSystem.current != null)
            {
                m_lastSelectedUIObject = EventSystem.current.currentSelectedGameObject;
                EventSystem.current.SetSelectedGameObject(m_input.gameObject);
            }
            if (!m_canvas.enabled && EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(m_lastSelectedUIObject);
            }
        }

        public void Clear()
        {
            m_output.text = string.Empty;
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
