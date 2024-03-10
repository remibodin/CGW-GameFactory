using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using TMPro;

using Cgw.Audio;
using Cgw.Assets;
using Cgw.Graphics;
using Cgw.Localization;

namespace Cgw.UI
{
    public class StartMenu : MonoBehaviour
    {
        [SerializeField] private Image m_background;
        [SerializeField] private Image m_logo;
        [SerializeField] private StartMenuPage m_homePage;
        [SerializeField] private StartMenuPage m_optionsPages;
        [SerializeField] private Button m_startBtn;
        [SerializeField] private Button m_optionsBtn;
        [SerializeField] private Button m_exitBtn;
        [SerializeField] private Button m_backBtn;
        [SerializeField] private TMP_Dropdown m_langDropDown;

        private UiImageBehaviour m_logoBehaviour;
        private Configuration m_configuration;

        private void Start()
        {
            m_optionsBtn.onClick.AddListener(OptionsBtn_OnClick);
            m_startBtn.onClick.AddListener(StartBtn_OnClick);
            m_backBtn.onClick.AddListener(BackBtn_OnClick);
            m_exitBtn.onClick.AddListener(ExitBtn_OnClick);

            m_langDropDown.ClearOptions();
            for (int i = 1; i <= LocalizationManager.Instance.Count; i++)
            {
                m_langDropDown.options.Add(new TMP_Dropdown.OptionData() 
                {
                    text = LocalizationManager.Instance.Get("LANG_NAME", i)
                });
            }
            m_langDropDown.onValueChanged.AddListener(LangDropDown_OnValueChange);

            m_optionsPages.Hide();
            m_homePage.Show();

            m_configuration = ResourcesManager.Get<Configuration>("configuration");
            if (m_configuration == null)
            {
                this.enabled = false;
                return;
            }
            m_configuration.OnUpdated += OnConfigurationUpdated;

            m_logoBehaviour = m_logo.gameObject.AddComponent<UiImageBehaviour>();
            m_logo.preserveAspect = true;

            UpdateConfiguration();
        }

        private void StartBtn_OnClick()
        {
            Fade.Out(() =>
            {
                SceneManager.LoadScene("LDTestScene"); 
                Fade.In();
            });
        }

        private void ExitBtn_OnClick()
        {
            Application.Quit();
        }

        private void OptionsBtn_OnClick()
        {
            m_optionsPages.Show();
            m_homePage.Hide();
        }

        private void BackBtn_OnClick()
        {
            m_optionsPages.Hide();
            m_homePage.Show();
        }

        private void LangDropDown_OnValueChange(int p_value)
        {
            LocalizationManager.Instance.SetLangageId(p_value);
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
            m_logoBehaviour.Asset = ResourcesManager.Get<SpriteAsset>(m_configuration.MenuLogoIdentifier);

            m_logo.enabled = m_logoBehaviour.Asset != null;
        }
    }

}