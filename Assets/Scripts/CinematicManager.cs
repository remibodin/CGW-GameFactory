using System;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Cgw
{
    public class CinematicManager
    {
        public static readonly CinematicManager Instance = new ();

        public static void Play(string p_identifier, Action p_callback)
        {
            var cinematicAsset = Resources.Load<Cinematic>(p_identifier);
            SceneManager.LoadSceneAsync("Cinematic").completed += (op) =>
            {
                var cinematicInstance = GameObject.Instantiate(cinematicAsset);
                cinematicInstance.Play(() => 
                {
                    GameObject.Destroy(cinematicInstance.gameObject);
                    p_callback?.Invoke();
                });
            };
        }
    }
}