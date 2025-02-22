using System.Runtime.CompilerServices;

using UnityEngine;

/// SingleBehaviourInScene try find objetc and can be null
/// SingleBehaviour create GameOject and T if needed

namespace Cgw
{
    public class SingleBehaviourInScene<T> : MonoBehaviour
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

        private static T GetInstance()
        {
            GameObject go = GameObject.FindFirstObjectByType(typeof(T)) as GameObject;
            if (go == null)
            {
                return null;
            }
            return go.GetComponent<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void Initialize()
        {
            if (_instance == null)
            {
                _instance = GetInstance();
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
    
