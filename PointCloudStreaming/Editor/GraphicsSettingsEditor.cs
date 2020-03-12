using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;

[InitializeOnLoad]
public class GraphicsSettingsEditor
{

    static BuildTarget[] buildTargets = new BuildTarget[]
    {
        BuildTarget.StandaloneWindows,
        BuildTarget.StandaloneWindows64,
        BuildTarget.StandaloneLinux64,
    };

    static GraphicsSettingsEditor ()
    {
        EditorApplication.update += CheckGraphicsAPIs;
    }

    static void CheckGraphicsAPIs()
    {        
        foreach (BuildTarget target in buildTargets)
        {
            if (PlayerSettings.GetUseDefaultGraphicsAPIs(target))
            {
                Debug.LogWarning("Changing default Graphics API for " + target.ToString() + " to OpenGL");
                PlayerSettings.SetUseDefaultGraphicsAPIs(target, false);
                PlayerSettings.SetGraphicsAPIs(target, new GraphicsDeviceType[] { GraphicsDeviceType.OpenGLCore });
                EditorApplication.Beep();
                Debug.LogWarning("Graphics settings changed, please restart editor");
            }
        }
    }

}