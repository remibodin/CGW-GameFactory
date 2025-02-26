using UnityEngine;

using Cgw.Assets;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Cgw.UI
{
    public class PostExposureValue : MonoBehaviour
    {
        private Configuration m_configuration;
        private Volume _volume;

        private void Start()
        {
            _volume = GetComponent<Volume>();

            m_configuration = ResourcesManager.Get<Configuration>("configuration");
            if (m_configuration == null)
            {
                this.enabled = false;
                return;
            }
            UpdateConfiguration();
            m_configuration.OnUpdated += OnConfigurationUpdated;
        }

        private void OnDisable()
        {
            m_configuration.OnUpdated -= OnConfigurationUpdated;
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
            if (_volume.profile.TryGet<ColorAdjustments>(out ColorAdjustments colorAdjustments))
            {
                colorAdjustments.postExposure.value = m_configuration.Exposure;
            }
        }
    }
}