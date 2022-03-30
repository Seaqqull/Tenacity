using UnityEngine;
using UnityEditor;

namespace Lovatto.SceneLoader.Editor
{
    public class bl_LoadingScreenWelcome : EditorWindow
    {

        [MenuItem("Window/LOS")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindowWithRect<bl_LoadingScreenWelcome>(new Rect(550, 400, 300, 125), true, "LOADING SCREEN", true);
        }

        void OnGUI()
        {
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical("box");
            GUIStyle gs = EditorStyles.helpBox;
            gs.richText = true;
            GUILayout.Label(("Thanks for use <b>Loading Screen</b>, \n for get started and documentation please check this:").ToUpper(), gs);
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("DOCUMENTATION", GUILayout.Height(40)))
            {
                //Application.OpenURL("http://lovattostudio.com/documentations/loading-screen/");
                GetWindow<LoadingScreenDocumentation>();
                this.Close();
            }
            GUILayout.FlexibleSpace();
        }
    }
}