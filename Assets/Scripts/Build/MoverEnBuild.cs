#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

class MyCustomBuildProcessor : IPostprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }
    public void OnPostprocessBuild(BuildReport report)
    {
        string path = report.summary.outputPath;
        string path2 = path.Replace(".exe", "_Data");
        string directorioA = Application.dataPath + "/Ejecutables_externos";
        string directorioB = path2 + "/Ejecutables_externos";
        FileUtil.CopyFileOrDirectory(directorioA, directorioB);
    }
}
#endif