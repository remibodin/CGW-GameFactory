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

        public void DelayAction(float delay, string action, params object[] args)
        {
            StartCoroutine(DelayInternal(delay, action, args));
        }

        private IEnumerator DelayInternal(float delay, string action, object[] args)
        {
            yield return new WaitForSeconds(delay);
            m_Instance.Call(action, args);
        }

        public void OnAnimEvent(string animEvent)
        {
            m_Instance.Call("OnAnimEvent", animEvent);
        }

        public void AddForceImpulse(Vector3 force)
        {
            var rigidbody = GetComponent<Rigidbody2D>();
            if (rigidbody != null)
            {
                rigidbody.AddForce(force, ForceMode2D.Impulse);
            }
        }
    }
}