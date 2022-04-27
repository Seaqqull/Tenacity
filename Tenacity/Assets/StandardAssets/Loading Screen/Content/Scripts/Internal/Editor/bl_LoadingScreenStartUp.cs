using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Lovatto.SceneLoader.Editor
{
    [InitializeOnLoad]
    class bl_LoadingScreenStartUp
    {

        private const string ONE_TIME_KEY = "lovatto.loadingscreen.startup";

        static bl_LoadingScreenStartUp()
        {
            if (PlayerPrefs.GetInt(ONE_TIME_KEY, 0) == 0)
            {
                EditorApplication.update += Update;
            }
        }

        [MenuItem("Window/Loading Screen/Add Levels")]
        public static void SetupLevelsOption()
        {
            Setup();
        }

        //[MenuItem("Window/Lovatto/Loading Screen/Documentation")]
        public static void OpenDocumentation()
        {
            //deprecated
            Application.OpenURL("http://lovattostudio.com/documentations/loading-screen/");
        }

        /// <summary>
        /// 
        /// </summary>
        static void Update()
        {
            PlayerPrefs.SetInt(ONE_TIME_KEY, 1);
            EditorApplication.update -= Update;

            bl_LoadingScreenWelcome.ShowWindow();
            Setup();
      
        }

        /// <summary>
        /// 
        /// </summary>
        static void Setup()
        {
            bl_SceneLoaderManager sm = Resources.Load("SceneLoaderManager", typeof(bl_SceneLoaderManager)) as bl_SceneLoaderManager;
            if (sm == null) { Debug.Log("Can't load scenes"); return; }

            SceneData[] allscenes = SceneNames();
            List<bl_SceneLoaderInfo> scenes = new List<bl_SceneLoaderInfo>();
            scenes.AddRange(sm.sceneList.ToArray());
            for (int i = 0; i < allscenes.Length; i++)
            {
                if (scenes.Exists(x => x.SceneName == allscenes[i].Name)) { continue; }
                bl_SceneLoaderInfo info = new bl_SceneLoaderInfo();
                info.SceneName = allscenes[i].Name;
                info.DisplayName = allscenes[i].Name;
                info.SceneAsset = AssetDatabase.LoadAssetAtPath(allscenes[i].Path, typeof(Object)) as Object;

                sm.sceneList.Add(info);
            }
            Debug.Log("Scenes setup with success!");

            Selection.objects = new UnityEngine.Object[] { sm };
            EditorUtility.SetDirty(sm);
            EditorGUIUtility.PingObject(sm);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SceneData[] SceneNames()
        {
            List<SceneData> temp = new List<SceneData>();
            foreach (EditorBuildSettingsScene S in EditorBuildSettings.scenes)
            {
                if (S.enabled)
                {
                    SceneData sd = new SceneData();
                    string name = S.path;
                    if (name.Contains("/"))
                        name = S.path.Substring(S.path.LastIndexOf('/') + 1);

                    if (string.IsNullOrEmpty(name)) continue;

                    name = name.Substring(0, name.Length - 6);
                    sd.Name = name;
                    sd.Path = S.path;
                    temp.Add(sd);
                }
            }
            return temp.ToArray();
        }

        public class SceneData
        {
            public string Name;
            public string Path;
        }
    }
}