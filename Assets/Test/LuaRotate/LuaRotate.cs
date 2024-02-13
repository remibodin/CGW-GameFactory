using UnityEngine;

using Cgw.Assets;
using Cgw.Scripting;

namespace Cgw.Test
{
    public class LuaRotate : MonoBehaviour
    {
        private void Start()
        {
            var script = ResourcesManager.Get<LuaScript>("Scripts/Rotate");
            var behaviour = gameObject.AddComponent<LuaBehaviour>();
            behaviour.Script = script;
        }
    }
}