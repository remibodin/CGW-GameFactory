using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Cgw.Assets;

namespace Cgw.UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private InputActionAsset m_inputActions = default;
        [SerializeField] private int[] m_excludedScenes = { 0 };
        [SerializeField] private Button m_resumeBtn = default;
        [SerializeField] private Button m_quitBtn = default;

        private CanvasGroup m_group;
        private bool m_paused = false;

        private void Awake()
        {
            m_group = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            m_inputActions.FindActionMap("Menu").FindAction("Pause").performed += SwitchPause;
            m_resumeBtn.onClick.AddListener(ResumeGame);
            m_quitBtn.onClick.AddListener(ReturnToStartMenu);
        }

        private void SwitchPause(InputAction.CallbackContext context)
        {
            if (m_paused)
                ResumeGame();
            else
                PauseGame();
        }

        private void PauseGame()
        {
            if (!CanPause())
                return;

            Time.timeScale = 0.0f;
            DisplayScreen(true);
            m_paused = true;
        }

        private void ResumeGame()
        {
            Time.timeScale = 1.0f;
            DisplayScreen(false);
            m_paused = false;
        }

        private void ReturnToStartMenu()
        {
            ResumeGame();
            SceneLoader.StartMenu();
        }

        private void DisplayScreen(bool p_visible)
        {
            m_group.alpha = p_visible ? 1.0f : 0.0f;
            m_group.blocksRaycasts = p_visible;
            m_group.interactable = p_visible;
        }

        private bool CanPause()
        {
            int curSceneIdx = SceneManager.GetActiveScene().buildIndex;

            foreach (var excludedSceneIdx in m_excludedScenes)
            {
                if (excludedSceneIdx == curSceneIdx)
                    return false;
            }

            return true;
        }
    }
}