using UnityEngine;

namespace Cgw
{
    public abstract class UniqueMonoBehaviour<T> : MonoBehaviour
    where T : MonoBehaviour
    {
        private static T m_instance = null;
        protected static T Instance => m_instance;

        protected virtual void Awake()
        {
            m_instance = this as T;
        }
    }
}
