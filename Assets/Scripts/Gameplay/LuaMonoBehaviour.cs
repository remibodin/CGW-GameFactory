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

        public void DelayAction(float delay, string action)
        {
            StartCoroutine(DelayInternal(delay, action));
        }

        private IEnumerator DelayInternal(float delay, string action)
        {
            yield return new WaitForSeconds(delay);
            m_Instance.Call(action);
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