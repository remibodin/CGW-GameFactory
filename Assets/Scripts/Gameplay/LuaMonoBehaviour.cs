using Cgw.Scripting;
using System.Collections;
using UnityEngine;

namespace Cgw.Gameplay
{
    public abstract class LuaMonoBehaviour : MonoBehaviour
    {
        protected LuaInstance m_Instance;

        protected virtual void OnAssetUpdate(LuaInstance instance)
        {
            m_Instance = instance;
        }

        public void DelayAction(string action, float delay)
        {
            StartCoroutine(DelayInternal(action, delay));
        }

        private IEnumerator DelayInternal(string action, float delay)
        {
            yield return new WaitForSeconds(delay);
            m_Instance.Call(action);
        }
    }
}