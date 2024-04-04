using Cgw.Assets;
using Cgw.Scripting;
using System;
using UnityEngine;

namespace Cgw.Gameplay
{
    public class CameraController : LuaEnvItem
    {
        public void Start()
        {
            var scriptBehaviour = gameObject.AddComponent<LuaBehaviour>();
            scriptBehaviour.OnAssetUpdated += ScriptBehaviour_OnAssetUpdated;
            scriptBehaviour.Script = ResourcesManager.Get<LuaScript>("Scripts/CameraController");
        }

        private void ScriptBehaviour_OnAssetUpdated(LuaInstance instance)
        {
            instance["this"] = this;
        }

        public void InitCameraPosition(Vector3 playerPosition, Vector3 facing, float cameraAheadPosition)
        {
            transform.position = new Vector3(playerPosition.x + facing.x * cameraAheadPosition, transform.position.y, -10.0f);
        }

        public void MoveCamera(float cameraOffset, float speed)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + (Vector3.right * cameraOffset), speed * Time.deltaTime);
        }
    }
}