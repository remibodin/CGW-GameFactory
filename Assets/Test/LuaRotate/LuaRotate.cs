using UnityEngine;

using Cgw.Assets;
using Cgw.Scripting;
using Cgw.Graphics;

namespace Cgw.Test
{
    public class LuaRotate : MonoBehaviour
    {
        private void Start()
        {
            var scriptBehaviour = gameObject.AddComponent<LuaBehaviour>();
            scriptBehaviour.Script = ResourcesManager.Get<LuaScript>("Scripts/Rotate");

            var spriteBehaviour = gameObject.AddComponent<SpriteBehaviour>();
            spriteBehaviour.Sprite = ResourcesManager.Get<SpriteAsset>("Sprites/Star");
        }
    }
}