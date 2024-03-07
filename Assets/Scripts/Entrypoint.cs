using System.Collections;

using UnityEngine;

using Cgw.Assets;
using Cgw.Assets.Loaders;
using Cgw.Scripting;
using Cgw.Graphics;
using Cgw.Audio;
using Cgw.Localization;

namespace Cgw
{
    public static class Entrypoint
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void BeforeSceneLoad()
        {
            CoroutineRunner.Initialize();

            ResourcesManager.SetProjectRoot("ExternalAssets");

            // Register loaders
            ResourcesManager.RegisterLoader<LuaScript>(new LuaScriptLoader());
            ResourcesManager.RegisterLoader<SpriteAsset>(new SpriteLoader());
            ResourcesManager.RegisterLoader<SoundAsset>(new SoundLoader());
            ResourcesManager.RegisterLoader<GameObjectAsset>(new GameObjectLoader());
            ResourcesManager.RegisterLoader<Configuration>(new YamlFileLoader<Configuration>());
            ResourcesManager.RegisterLoader<CSVFileAsset>(new CSVFileLoader());
            //

            GameObject.DontDestroyOnLoad(GameObject.Instantiate(Resources.Load("Fade")));
            GameObject.DontDestroyOnLoad(GameObject.Instantiate(Resources.Load("Terminal")));

            LocalizationManager.Instance.SetResourceIdentifier("Configurations/Localization");

            CoroutineRunner.StartCoroutine(SyncResourcesManager());

            AudioManager.Instance.Init();
        }

        // Sync assets every second
        static IEnumerator SyncResourcesManager()
        {
            while (true)
            {
                ResourcesManager.Sync();
                yield return new WaitForSeconds(1f);
            }
        }
    }
}