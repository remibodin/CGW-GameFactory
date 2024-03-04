using System;

namespace Cgw.Localization
{
    public class LocalizedString
    {
        public Action OnUpdated;

        public string Value { get; private set; }

        private readonly string m_key;

        public LocalizedString(string p_key)
        {
            m_key = p_key;
            Value = LocalizationManager.Instance.Get(m_key);
            LocalizationManager.Instance.onLangageUpdated += OnLangageUpdated;
        }

        private void OnLangageUpdated()
        {
            Value = LocalizationManager.Instance.Get(m_key);
            OnUpdated?.Invoke();
        }
    }
}