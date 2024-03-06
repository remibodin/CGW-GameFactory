using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

using Cgw.Assets;
using Cgw.Localization;
using Cgw.Scripting;

namespace Cgw
{
    public static class TerminalBuiltinCommands
    {
        [TermCommand]
        public static void Print(string p_args)
        {
            Debug.Log(p_args);
        }

        [TermCommand]
        public static void Clear(string p_args)
        {
            Terminal.Instance.Clear();
        }

        [TermCommand]
        public static void Help(string p_args)
        {
            Debug.Log($"<b>Commands list</b>:\n{string.Join('\n', Terminal.Instance.CommandsName)}");
        }

        [TermCommand]
        public static void Reset(string p_args)
        {
            SceneManager.LoadScene(0);
        }

        [TermCommand]
        public static void SetLang(string p_args)
        {
            LocalizationManager.Instance.SetLangage(p_args);
        }

        [TermCommand]
        public static void R(string p_args)
        {
            SceneManager.LoadScene("Empty");

            CoroutineRunner.StartCoroutine(CO_InitScene());
        }

        public static IEnumerator CO_InitScene()
        {
            yield return null;

            var scenePrefab = Resources.Load("Levels/Level-Test");
            var sceneObject = GameObject.Instantiate(scenePrefab);

            var playerPrefab = Resources.Load("Player");
            var playerObject = GameObject.Instantiate(playerPrefab) as GameObject;

            var spawnPoint = GameObject.FindWithTag("Respawn");
            playerObject.transform.position = spawnPoint.transform.position;
        }

        [TermCommand]
        public static void LoadLevel(string p_args)
        {
            SceneManager.LoadScene("Empty");

            CoroutineRunner.StartCoroutine(CO_InitLevel(p_args));
        }

        public static IEnumerator CO_InitLevel(string p_args)
        {
            yield return null;

            var LD = ResourcesManager.Get<GameObjectAsset>($"Levels/{p_args}");
            var sceneObject = GameObject.Instantiate(LD.GameObject);

            var playerPrefab = Resources.Load("Player");
            var playerObject = GameObject.Instantiate(playerPrefab) as GameObject;

            var spawnPoint = GameObject.FindWithTag("Respawn");
            playerObject.transform.position = spawnPoint.transform.position;
        }
    }
}
