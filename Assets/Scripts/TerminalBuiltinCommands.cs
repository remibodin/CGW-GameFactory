using UnityEngine;
using UnityEngine.SceneManagement;

using Cgw.Assets;
using Cgw.Localization;
using Cgw.Gameplay;

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
        public static void Spawn(string p_args)
        {
            Player player = GameObject.FindFirstObjectByType<Player>();
            if (player == null)
            {
                UnityEngine.Debug.LogWarning("Player not found in scene");
                return;
            }
            GameObject go = null;
            switch (p_args)
            {
                case "mu" : 
                    go = Resources.Load("Enemies/Mushroom") as GameObject;
                break;
                default:
                    go = Resources.Load(p_args) as GameObject;
                break;
            }
            if (go == null)
            {
                UnityEngine.Debug.LogWarning("Resource not found");
                return;
            }
            GameObject.Instantiate(go, player.transform.position + Vector3.right * 3, Quaternion.identity);
        }
    }
}
