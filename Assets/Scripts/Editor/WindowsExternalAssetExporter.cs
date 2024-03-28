using System.IO;

using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Cgw.Editor
{
    public class WindowsExternalAssetExporter : IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPostprocessBuild(BuildReport p_report)
        {
            string externalAssetsBuildPath = null;

            switch (p_report.summary.platform)
            {
                case BuildTarget.StandaloneWindows :
                case BuildTarget.StandaloneWindows64 :
                {
                    var buildPath = Path.GetFullPath(p_report.summary.outputPath);
                    var fileInfo = new FileInfo(buildPath);
                    externalAssetsBuildPath = Path.Combine(fileInfo.Directory.FullName, "ExternalAssets");
                }
                break;
                default:
                    return;
            }

            var externalAssetsPath = Path.GetFullPath("ExternalAssets");
            var externalAssetsInfos = new DirectoryInfo(externalAssetsPath);
            Debug.Log($"[ExternalAssetExporter] Copy external assets (externalAssetsPath={externalAssetsPath}, externalAssetsBuildPath={externalAssetsBuildPath})");
            externalAssetsInfos.DeepCopy(externalAssetsBuildPath);
        }
    }
}