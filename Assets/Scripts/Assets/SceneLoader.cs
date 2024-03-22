using UnityEngine.SceneManagement;

using Cgw.Graphics;

namespace Cgw.Assets
{
    /// <summary>
    /// Helper class to load scenes with fade in and out.
    /// </summary>
    public class SceneLoader : UniqueMonoBehaviour<SceneLoader>
    {
        public static void Level1()
        {
            Fade.In(() => LoadScene(1));
        }

        public static void StartMenu()
        {
            Fade.In(() => LoadScene(0));
        }

        public static void Custom(string p_sceneName)
        {
            Fade.In(() => LoadScene(p_sceneName));
        }

        public static void Custom(int p_sceneIndex)
        {
            Fade.In(() => LoadScene(p_sceneIndex));
        }

        private static void LoadScene(string p_sceneName)
        {
            SceneManager.LoadScene(p_sceneName);
            Fade.Out();
        }

        private static void LoadScene(int p_sceneIndex)
        {
            SceneManager.LoadScene(p_sceneIndex);
            Fade.Out();
        }
    }
}