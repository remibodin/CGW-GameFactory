using System.Runtime.CompilerServices;

using UnityEngine;

namespace Cgw
{
    public class SingleBehaviour<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        public static T Instance
        {
            get
            {
                Initialize();
                return _instance;
            }
        }

        protected static T _instance = null;

        private static T CreateInstance()
        {
            GameObject go = new GameObject(typeof(T).Name);
            DontDestroyOnLoad(go);
            return go.AddComponent<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void Initialize()
        {
            if (_instance == null)
            {
                _instance = CreateInstance();
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
            }
        }
    }
}
    
