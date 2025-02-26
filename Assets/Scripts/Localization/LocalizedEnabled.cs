using UnityEngine;

namespace Cgw.Localization
{
    // enable component for specific langage
    public class LocalizedEnabled : MonoBehaviour
    {
        public string Langage;
        public Behaviour component;

        private int _langageId;

        private void Start()
        {
            _langageId = LocalizationManager.Instance.GetLangageId(Langage);
            OnLangageUpdated();
            LocalizationManager.Instance.onLangageUpdated += OnLangageUpdated;
        }

        private void OnLangageUpdated()
        {
            component.enabled = LocalizationManager.Instance.GetLangageId() == _langageId;
        }
    }
}