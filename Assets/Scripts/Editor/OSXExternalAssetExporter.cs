using System.IO;

using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Cgw.Editor
{
    public class OSXExternalAssetExporter : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport p_report)
        {
            if (p_report.summary.platform != BuildTarget.StandaloneOSX)
            {
                return;
            }

            var streamingAssetsPath = Path.Combine(Application.dataPath, "StreamingAssets");
            if (!Directory.Exists(streamingAssetsPath))
            {
                Directory.CreateDirectory(streamingAssetsPath);
            }
            var externalAssetsBuildPath = Path.Combine(streamingAssetsPath, "ExternalAssets");
            var externalAssetsPath = Path.GetFullPath("ExternalAssets");
            var externalAssetsInfos = new DirectoryInfo(externalAssetsPath);
            Debug.Log($"[ExternalAssetExporter] Copy external assets (externalAssetsPath={externalAssetsPath}, externalAssetsBuildPath={externalAssetsBuildPath})");
            externalAssetsInfos.DeepCopy(externalAssetsBuildPath);
            AssetDatabase.Refresh();
        }

        public void OnPostprocessBuild(BuildReport p_report)
        {
            if (p_report.summary.platform != BuildTarget.StandaloneOSX)
            {
                return;
            }

            var streamingAssetsPath = Path.Combine(Application.dataPath, "StreamingAssets");
            var externalAssetsBuildPath = Path.Combine(streamingAssetsPath, "ExternalAssets");
            if (Directory.Exists(streamingAssetsPath))
            {
                Directory.Delete(externalAssetsBuildPath, true);
            }
            AssetDatabase.Refresh();
        }
    }
}