using UnityEngine;
using UnityEngine.UI;

using Cgw.Audio;
using Cgw.Assets;
using Cgw.Graphics;

namespace Cgw
{
    public class StartMenu : MonoBehaviour
    {
        [SerializeField] private Image m_background;
        [SerializeField] private Image m_logo;
        [SerializeField] private Button m_startBtn;
        [SerializeField] private Button m_optionsBtn;
        [SerializeField] private Button m_exitBtn;

        private SoundBehaviour m_selectedSound;
        private SoundBehaviour m_loopSound;
        private UiImageBehaviour m_logoBehaviour;
        private Configuration m_configuration;

        private void Start()
        {
            m_exitBtn.onClick.AddListener(ExitBtn_OnClick);

            m_configuration = ResourcesManager.Get<Configuration>("configuration");
            if (m_configuration == null)
            {
                this.enabled = false;
                return;
            }
            m_configuration.OnUpdated += OnConfigurationUpdated;

            m_selectedSound = new GameObject("[Sound] Selected").AddComponent<SoundBehaviour>();
            m_loopSound = new GameObject("[Sound] Loop").AddComponent<SoundBehaviour>();

            m_logoBehaviour = m_logo.gameObject.AddComponent<UiImageBehaviour>();
            m_logo.preserveAspect = true;

            UpdateConfiguration();
        }

        public void PlaySelectedSound()
        {
            m_selectedSound?.Source.Play();
        }

        private void ExitBtn_OnClick()
        {
            Application.Quit();
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
            if (ColorUtility.TryParseHtmlString(m_configuration.MenuBackgroundColor, out var color))
            {
                m_background.color = color;
            }
            m_loopSound.Asset = ResourcesManager.Get<SoundAsset>(m_configuration.MenuLoopButtonSfxIdentifier);
            m_selectedSound.Asset = ResourcesManager.Get<SoundAsset>(m_configuration.MenuSelectButtonSfxIdentifier);
            m_logoBehaviour.Asset = ResourcesManager.Get<SpriteAsset>(m_configuration.MenuLogoIdentifier);

            m_logo.enabled = m_logoBehaviour.Asset != null;
        }
    }

}