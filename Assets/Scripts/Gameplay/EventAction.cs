using UnityEngine;

namespace Cgw.Gameplay
{
    public abstract class EventAction : ScriptableObject
    {
        public abstract void OnActivate();
        public abstract void OnUpdate();
    }
}