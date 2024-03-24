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
            Fade.Out(() => PlayCinematicAndLoadScene("Cinematics/Intro", 1));
        }

        public static void StartMenu()
        {
            Fade.Out(() => LoadScene(0));
        }

        public static void Custom(string p_sceneName)
        {
            Fade.Out(() => LoadScene(p_sceneName));
        }

        public static void Custom(int p_sceneIndex)
        {
            Fade.Out(() => LoadScene(p_sceneIndex));
        }

        private static void LoadScene(string p_sceneName)
        {
            SceneManager.LoadScene(p_sceneName);
            Fade.In();
        }

        private static void PlayCinematicAndLoadScene(string p_cinematicIdentifier, string p_sceneName)
        {
            CinematicManager.Play(p_cinematicIdentifier, () => LoadScene(p_sceneName));
        }

        private static void PlayCinematicAndLoadScene(string p_cinematicIdentifier, int p_sceneIndex)
        {
            CinematicManager.Play(p_cinematicIdentifier, () => LoadScene(p_sceneIndex));
        }

        private static void LoadScene(int p_sceneIndex)
        {
            SceneManager.LoadScene(p_sceneIndex);
            Fade.In();
        }
    }
}