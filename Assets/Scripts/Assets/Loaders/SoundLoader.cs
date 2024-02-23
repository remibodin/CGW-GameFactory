using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

using Cgw.Audio;

namespace Cgw.Assets.Loaders
{
    public class SoundLoader : AssetFileLoader<SoundAsset>
    {
        private Dictionary<string, AudioType> m_audioTypePerExtention = new Dictionary<string, AudioType>()
        {
            {"mp3", AudioType.MPEG},
            {"ogg", AudioType.OGGVORBIS},
            {"wav", AudioType.WAV}
        };

        public override IEnumerable<string> Extentions => m_audioTypePerExtention.Keys;

        public override SoundAsset LoadAsset(string p_metadataPath, string p_filePath, SoundAsset p_data)
        {
            if (p_filePath == null)
            {
                throw new System.Exception("File not found");
            }
            p_data.LoadingCoroutine = CoroutineRunner.StartCoroutine(CO_LoadAsset(p_metadataPath, p_filePath, p_data));
            return p_data;
        }

        private IEnumerator CO_LoadAsset(string p_metadataPath, string p_filePath, SoundAsset p_data)
        {
            var extention = Path.GetExtension(p_filePath).Substring(1); // Substring 1 to remove the dot char at the begining of the extention
            
            if (!m_audioTypePerExtention.TryGetValue(extention, out var audioType))
            {
                Debug.LogWarning($"SoundLoader: audio type not found for this extention (extention={extention})");
                audioType = AudioType.UNKNOWN;
            }

            using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(p_filePath, audioType))
            {
                yield return request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log(request.error);
                }
                else
                {
                    p_data.AudioClip = DownloadHandlerAudioClip.GetContent(request);
                }
            }
            p_data.LoadingCoroutine = null;
        }
    }
}
