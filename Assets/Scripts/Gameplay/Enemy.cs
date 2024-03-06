using UnityEngine;

namespace Cgw.Gameplay
{
    public abstract class Enemy : MonoBehaviour
    {
        public abstract void Attacked(float power);
    }
}