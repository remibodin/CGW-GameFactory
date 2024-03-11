using UnityEngine;

namespace Cgw.Gameplay
{
    public abstract class Enemy : LuaMonoBehaviour
    {
        public abstract void Attacked(float power);
        public abstract void OnCollisionWithPlayer();
        public abstract void OnCollisionWithSpider();
        public abstract void OnCollisionWithDanger();
    }
}