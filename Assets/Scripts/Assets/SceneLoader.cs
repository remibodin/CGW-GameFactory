using UnityEngine.SceneManagement;

using Cgw.Graphics;

namespace Cgw.Assets
{
    /// <summary>
    /// Helper class to load scenes with fade in and out.
    /// </summary>
    public class SceneLoader : SingleBehaviour<SceneLoader>
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

        [TermCommand]
        private static void LoadScene(string p_sceneName)
        {
            SceneManager.LoadScene(p_sceneName);
            Fade.In();
        }

        public static void PlayCinematicAndLoadScene(string p_cinematicIdentifier, string p_sceneName)
        {
            CinematicManager.Play(p_cinematicIdentifier, () => LoadScene(p_sceneName));
        }

        public static void PlayCinematicAndLoadScene(string p_cinematicIdentifier, int p_sceneIndex)
        {
            CinematicManager.Play(p_cinematicIdentifier, () => LoadScene(p_sceneIndex));
        }

        private static void LoadScene(int p_sceneIndex)
        {
            SceneManager.LoadScene(p_sceneIndex);
            Fade.In();
        }

        [TermCommand]
        private static void Reset(string p_args)
        {
            if (!string.IsNullOrEmpty(p_args) &&
                p_args.ToLower() == "hard")
            {
                SceneManager.LoadScene(0);
                return;
            }
            SceneLoader.StartMenu();
        }
    }
}