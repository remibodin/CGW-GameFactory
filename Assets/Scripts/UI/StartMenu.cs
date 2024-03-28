using UnityEngine;
using UnityEngine.UI;

using TMPro;

using Cgw.Audio;
using Cgw.Assets;
using Cgw.Graphics;
using Cgw.Localization;
using System.Collections.Generic;

namespace Cgw.UI
{
    public class StartMenu : MonoBehaviour
    {
        [SerializeField] private Image m_background;
        [SerializeField] private Image m_logo;
        [SerializeField] private Image m_logoLighting;
        [SerializeField] private Image m_title;
        [SerializeField] private Image m_titleHaloFx;
        [SerializeField] private StartMenuPage m_homePage;
        [SerializeField] private StartMenuPage m_optionsPages;

        [Header("Buttons")]
        [SerializeField] private Button m_startBtn;
        [SerializeField] private Button m_optionsBtn;
        [SerializeField] private Button m_exitBtn;
        [SerializeField] private Button m_backBtn;

        [Header("Options")]
        [SerializeField] private TMP_Dropdown m_langDropDown;
        [SerializeField] private TMP_Dropdown m_resolutionDropDown;
        [SerializeField] private Toggle m_fullscreenToggle;

        private SoundBehaviour m_selectedSound;
        private SoundBehaviour m_loopSound;
        private UiImageBehaviour m_logoBehaviour;
        private UiImageBehaviour m_logoLightingBehaviour;
        private UiImageBehaviour m_titleBehaviour;
        private UiImageBehaviour m_titleHaloFxBehaviour;
        private Configuration m_configuration;

        private List<Resolution> m_availableResolutions;

        private void Start()
        {
            m_optionsBtn.onClick.AddListener(OptionsBtn_OnClick);
            m_backBtn.onClick.AddListener(BackBtn_OnClick);
            m_exitBtn.onClick.AddListener(ExitBtn_OnClick);
            m_startBtn.onClick.AddListener(StartBtn_OnClick);

            m_langDropDown.ClearOptions();
            for (int i = 1; i <= LocalizationManager.Instance.Count; i++)
            {
                m_langDropDown.options.Add(new TMP_Dropdown.OptionData() 
                {
                    text = LocalizationManager.Instance.Get("LANG_NAME", i)
                });
            }
            m_langDropDown.onValueChanged.AddListener(LangDropDown_OnValueChange);

            InitScreenOptions();

            m_optionsPages.ForceHide();
            m_homePage.Show();

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

            m_logoLightingBehaviour = m_logoLighting.gameObject.AddComponent<UiImageBehaviour>();
            m_logoLighting.preserveAspect = true;

            m_titleBehaviour = m_title.gameObject.AddComponent<UiImageBehaviour>();
            m_title.preserveAspect = true;

            m_titleHaloFxBehaviour = m_titleHaloFx.gameObject.AddComponent<UiImageBehaviour>();
            m_titleHaloFx.preserveAspect = true;

            UpdateConfiguration();
        }

        private void InitScreenOptions()
        {
            // Resolutions
            m_resolutionDropDown.ClearOptions();
            m_availableResolutions = new List<Resolution>();

            var resolutions = Screen.resolutions;
            var curRes = new Resolution
            {
                // Using player window resolution so fullscreen state doesn't matter anymore
                // Still use Screen refresh rate
                width = Screen.width, 
                height = Screen.height,
                refreshRateRatio = Screen.currentResolution.refreshRateRatio
            };

            int selectedRes = 0;
            foreach (var resolution in resolutions)
            {
                if (resolution.refreshRateRatio.value != curRes.refreshRateRatio.value)
                {
                    continue;
                }

                if (resolution.width == curRes.width &&
                    resolution.height == curRes.height)
                {
                    selectedRes = m_availableResolutions.Count;
                }

                m_availableResolutions.Add(resolution);
                m_resolutionDropDown.options.Add(new TMP_Dropdown.OptionData() 
                {
                    text = $"{resolution.width} x {resolution.height}"
                });
            }
            m_resolutionDropDown.value = selectedRes;
            m_resolutionDropDown.onValueChanged.AddListener(ResolutionDropDown_OnValueChange);

            // Fullscreen state
            bool isFullscreen = Screen.fullScreen;
            m_fullscreenToggle.isOn = isFullscreen;
            m_fullscreenToggle.onValueChanged.RemoveAllListeners();
            m_fullscreenToggle.onValueChanged.AddListener(FullscreenToggle_OnValueChanged);
        }

        public void PlaySelectedSound()
        {
            m_selectedSound?.Source.Play();
        }

        private void ExitBtn_OnClick()
        {
            Application.Quit();
        }

        private void OptionsBtn_OnClick()
        {
            float delay = m_homePage.Hide();
            m_optionsPages.Show(delay);
        }

        private void BackBtn_OnClick()
        {
            float delay = m_optionsPages.Hide();
            m_homePage.Show(delay);
        }

        private void StartBtn_OnClick()
        {
            SceneLoader.Level1();
        }

        private void LangDropDown_OnValueChange(int p_value)
        {
            LocalizationManager.Instance.SetLangageId(p_value);
        }

        private void ResolutionDropDown_OnValueChange(int p_value)
        {
            if (p_value > m_availableResolutions.Count)
            {
                Debug.LogError("Resolution Error");
                return;
            }

            var newRes = m_availableResolutions[p_value];
            Screen.SetResolution(newRes.width, newRes.height, m_fullscreenToggle.isOn);
        }

        private void FullscreenToggle_OnValueChanged(bool p_selected)
        {
            if (p_selected != Screen.fullScreen)
            {
                Screen.fullScreen = p_selected;
            }
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
            m_logoLightingBehaviour.Asset = ResourcesManager.Get<SpriteAsset>(m_configuration.MenuLogoLightingIdentifier);
            m_titleBehaviour.Asset = ResourcesManager.Get<SpriteAsset>(m_configuration.MenuTitleIdentifier);
            m_titleHaloFxBehaviour.Asset = ResourcesManager.Get<SpriteAsset>(m_configuration.MenuHaloFXIdentifier);

            m_logo.enabled = m_logoBehaviour.Asset != null;
            m_logoLighting.enabled = m_logoLightingBehaviour.Asset != null && m_logo.enabled;
            m_title.enabled = m_titleBehaviour.Asset != null;
            m_titleHaloFx.enabled = m_titleHaloFxBehaviour.Asset != null;
        }
    }
}