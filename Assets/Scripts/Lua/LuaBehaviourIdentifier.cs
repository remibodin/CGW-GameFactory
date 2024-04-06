using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Cgw.Assets;

namespace Cgw.Scripting
{
    public class LuaBehaviourIdentifier : LuaBehaviour
    {
        [SerializeField]
        private string m_identifier;

        protected override void Awake()
        {
            Script = ResourcesManager.Get<LuaScript>(m_identifier);
            base.Awake();
        }
    }
}
