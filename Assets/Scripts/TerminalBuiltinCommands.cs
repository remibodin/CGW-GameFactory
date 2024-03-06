using UnityEngine;
using UnityEngine.SceneManagement;

using Cgw.Localization;

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
    }
}
