using System.IO;

using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Cgw.Editor
{
    public class ExternalAssetExporter : IPostprocessBuildWithReport
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
                case BuildTarget.StandaloneOSX :
                {
                    var buildPath = Path.GetFullPath(p_report.summary.outputPath);
                    var directoryInfo = new DirectoryInfo(buildPath);
                    externalAssetsBuildPath = Path.Combine(directoryInfo.FullName, "Contents", "ExternalAssets");
                }
                break;
            }

            if (externalAssetsBuildPath == null)
            {
                Debug.LogWarning($"[ExternalAssetExporter] {p_report.summary.platform} not supported");
                return;
            }

            var externalAssetsPath = Path.GetFullPath("ExternalAssets");
            var externalAssetsInfos = new DirectoryInfo(externalAssetsPath);
            externalAssetsInfos.DeepCopy(externalAssetsBuildPath);
        }
    }
}