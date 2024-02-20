using System.Collections;

using UnityEngine;

using Cgw.Assets;
using Cgw.Assets.Loaders;
using Cgw.Scripting;
using Cgw.Graphics;
using Cgw.Audio;
using System.Resources;
using Assets.Nodes;

namespace Cgw
{
    public static class Entrypoint
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void BeforeSceneLoad()
        {
            CoroutineRunner.Initialize();

            ResourcesManager.SetProjectRoot("ExternalAssets");

            GameObject.DontDestroyOnLoad(GameObject.Instantiate(Resources.Load("Fade")));
            GameObject.DontDestroyOnLoad(GameObject.Instantiate(Resources.Load("Terminal")));

            // Register loaders
            ResourcesManager.RegisterLoader<LuaScript>(new LuaScriptLoader());
            ResourcesManager.RegisterLoader<SpriteAsset>(new SpriteLoader());
            ResourcesManager.RegisterLoader<SoundAsset>(new SoundLoader());
            ResourcesManager.RegisterLoader<GameObjectAsset>(new GameObjectLoader());
            ResourcesManager.RegisterLoader<Configuration>(new YamlFileLoader<Configuration>());
            ResourcesManager.RegisterLoader<NodeCollection>(new NodeCollectionLoader());
            ResourcesManager.RegisterLoader<EntrypointAsset>(new EntrypointAssetLoader());
            ResourcesManager.RegisterLoader<ConditionAsset>(new ConditionAssetLoader());
            ResourcesManager.RegisterLoader<InOutFlowAsset>(new InOutFlowAssetLoader());
            //

            CoroutineRunner.StartCoroutine(SyncResourcesManager());
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