using System.Collections;

using UnityEngine;

using Cgw.Assets;
using Cgw.Assets.Loaders;
using Cgw.Scripting;
using Cgw.Graphics;

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