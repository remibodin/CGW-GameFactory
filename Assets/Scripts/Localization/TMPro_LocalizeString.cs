using UnityEngine;

using TMPro;

namespace Cgw.Localization
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TMProLocalizeString : MonoBehaviour
    {
        public string LocalizationKey;

        private LocalizedString m_localizedString;
        private TextMeshProUGUI m_TMPro;

        private void Awake()
        {
            m_localizedString = new LocalizedString(LocalizationKey);
        }

        private void Start()
        {
            m_TMPro = GetComponent<TextMeshProUGUI>();
            if (m_TMPro == null)
            {
                enabled = false;
                return;
            }
            m_TMPro.text = m_localizedString.Value;
            m_localizedString.OnUpdated = () => m_TMPro.text = m_localizedString.Value;
        }
    }
}