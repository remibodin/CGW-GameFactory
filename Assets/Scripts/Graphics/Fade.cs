using UnityEngine;
using UnityEngine.UI;

using Cgw.Assets;
using System;

namespace Cgw.Graphics
{
    public class Fade : UniqueMonoBehaviour<Fade>
    {
        [SerializeField] private Image m_image;
        [SerializeField] private float m_speed = 1f;

        public Color BaseColor = Color.black;

        private float m_timeTarget = 0;
        private float m_timeCurrent = 0;
        private Action m_endAction;
        private Configuration m_configuration;

        private void Start()
        {
            m_configuration = ResourcesManager.Get<Configuration>("configuration");
            m_configuration.OnUpdated += OnConfigurationUpdated;
            UpdateConfiguration();
        }

        private void OnConfigurationUpdated(Asset p_newConfiguration)
        {
            m_configuration.OnUpdated -= OnConfigurationUpdated;
            m_configuration = p_newConfiguration as Configuration;
            m_configuration.OnUpdated += OnConfigurationUpdated;
            UpdateConfiguration();
        }

        private void UpdateConfiguration()
        {
            if (ColorUtility.TryParseHtmlString(m_configuration.FadeColor, out var color))
            {
                BaseColor = color;
            }
            m_speed = m_configuration.FadeSpeed;
        }

        private void SetAlphaValue(float p_alpha)
        {
            var color = m_image.color;
            color.r = BaseColor.r;
            color.g = BaseColor.g;
            color.b = BaseColor.b;
            color.a = p_alpha;
            m_image.color = color;
        }

        private float BezierBlend(float t)
        {
            return t * t * (3.0f - 2.0f * t);
        }

        private void Update()
        {
            var direction = m_timeCurrent > m_timeTarget ? -1 : 1;
            m_timeCurrent = m_timeCurrent + Time.deltaTime * direction * m_speed;
            m_timeCurrent = Mathf.Clamp01(m_timeCurrent);
            SetAlphaValue(BezierBlend(m_timeCurrent));

            if (m_timeCurrent == m_timeTarget &&
                m_endAction != null)
            {
                m_endAction.Invoke();
                m_endAction = null;
            }
        }

        public static void In(Action p_callback = null)
        {
            Fade.Instance.m_timeTarget = 0;
            Fade.Instance.m_endAction = p_callback;
        }

        public static void Out(Action p_callback = null)
        {
            Fade.Instance.m_timeTarget = 1;
            Fade.Instance.m_endAction = p_callback;

        }

        public static void Toggle(Action p_callback = null)
        {
            if (Fade.Instance.m_timeTarget == 0) 
                Fade.Instance.m_timeTarget = 1;
            else 
                Fade.Instance.m_timeTarget = 0;
            Fade.Instance.m_endAction = p_callback;
        }
    }
}