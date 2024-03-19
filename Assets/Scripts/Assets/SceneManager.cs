using UnityEngine.SceneManagement;

using Cgw.Graphics;

namespace Cgw.Assets
{
    /// <summary>
    /// Helper class to load scenes with fade in and out.
    /// </summary>
    public class SceneLoader : UniqueMonoBehaviour<SceneLoader>
    {
        public static void DarkForest()
        {
            Fade.In(() => LoadScene("DarkForest"));
        }

        public static void StartMenu()
        {
            Fade.In(() => LoadScene("StartMenu"));
        }

        public static void Custom(string p_sceneName)
        {
            Fade.In(() => LoadScene(p_sceneName));
        }

        private static void LoadScene(string p_sceneName)
        {
            SceneManager.LoadScene(p_sceneName);
            Fade.Out();
        }
    }
}