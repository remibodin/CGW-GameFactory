using UnityEngine;

namespace Cgw.Assets
{
    public static class PrefabSpawner
    {
        public static void SpawnPrefab(string prefabPath, Vector3 position)
        {

        }

        public static void SpawnPrefab(string prefabPath, Transform transform)
        {
            SpawnPrefab(prefabPath, transform.position);
        }

        public static void SpawnPrefab(string prefabPath, GameObject gameObject)
        {
            SpawnPrefab(prefabPath, gameObject.transform.position);
        }
    }
}