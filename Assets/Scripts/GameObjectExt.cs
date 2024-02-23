using UnityEngine;

namespace Cgw
{
    public static class GameObjectExt
    {
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T component;
            if (!gameObject.TryGetComponent<T>(out component))
            {
                component = gameObject.AddComponent<T>();
            }
            return component;
        }
    }
}
