using System.Collections;

using UnityEngine;

namespace Cgw
{
    public class CoroutineRunner
    {
        public class Runner : MonoBehaviour { }

        private static Runner m_runner;

        public static void Initialize()
        {
            var go = new GameObject("CoroutineRunner");
            GameObject.DontDestroyOnLoad(go);
            m_runner = go.AddComponent<Runner>();
        }

        public static Coroutine StartCoroutine(IEnumerator p_coroutine)
        {
            return m_runner.StartCoroutine(p_coroutine);
        }

        public static void StopCoroutine(Coroutine p_coroutine)
        {
            m_runner.StopCoroutine(p_coroutine);
        }
    }
}