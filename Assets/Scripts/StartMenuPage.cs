using UnityEngine;

namespace Cgw
{
    public class StartMenuPage : MonoBehaviour
    {
        [SerializeField] private CanvasGroup m_group;
        [SerializeField] private GameObject m_firstSelectedControl;

        public void Show()
        {
            m_group.alpha = 1;
            m_group.interactable = true;
            // m_group.blockRaycasts = true;
        }

        public void Hide()
        {
            m_group.alpha = 0;
            m_group.interactable = false;
            // m_group.blockRaycasts = false;
        }
    }
}