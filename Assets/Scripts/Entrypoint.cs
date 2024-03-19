using System.Collections;
using System.IO;

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

            string externalAssetsPAth = "ExternalAssets";
#if UNITY_STANDALONE_OSX && !UNITY_EDITOR
            externalAssetsPAth = Path.Combine(Application.dataPath, externalAssetsPAth);
#endif

            ResourcesManager.SetProjectRoot(externalAssetsPAth);

            // Register loaders
            ResourcesManager.RegisterLoader<LuaScript>(new LuaScriptLoader());
            ResourcesManager.RegisterLoader<SpriteAsset>(new SpriteLoader());
            ResourcesManager.RegisterLoader<SoundAsset>(new SoundLoader());
            ResourcesManager.RegisterLoader<SoundAssetCollection>(new YamlFileLoader<SoundAssetCollection>());
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