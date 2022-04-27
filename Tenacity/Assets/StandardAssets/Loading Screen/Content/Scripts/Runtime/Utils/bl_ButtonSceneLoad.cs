using UnityEngine;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Lovatto.SceneLoader
{
    public class bl_ButtonSceneLoad : MonoBehaviour, IPointerClickHandler
    {
        public bool byName = true;
        public string sceneName;
        public int sceneID = 0;

        public void OnPointerClick(PointerEventData eventData)
        {
            LoadScene();
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadScene()
        {
            if (byName)
            {
                if (string.IsNullOrEmpty(sceneName)) return;
                bl_SceneLoaderManager.LoadScene(sceneName);
            }
            else
            {
                bl_SceneLoaderManager.LoadSceneByID(sceneID);
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(bl_ButtonSceneLoad))]
    public class bl_ButtonSceneLoadEditor : Editor
    {
        bl_ButtonSceneLoad script;
        string[] sceneNames;

        private void OnEnable()
        {
            script = (bl_ButtonSceneLoad)target;
            sceneNames = bl_SceneLoaderManager.Instance.GetSceneNames();
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.BeginVertical("box");
            {
                script.byName = EditorGUILayout.ToggleLeft("By Name", script.byName, EditorStyles.toolbarButton);
                GUILayout.Space(2);
                if (script.byName)
                {
                    script.sceneName = EditorGUILayout.TextField("Scene Name", script.sceneName);
                }
                else
                    script.sceneID = EditorGUILayout.Popup("Scene", script.sceneID, sceneNames, EditorStyles.toolbarPopup);
            }
            GUILayout.Space(2);
            EditorGUILayout.EndVertical();

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
            }
        }
    }
#endif
}