using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using Lovatto.SceneLoader;

public class bl_SceneLoaderManager : ScriptableObject
{

    public LoadingScreenMode loadingScreenMode = LoadingScreenMode.OnePerScene;
    [Header("Scene Manager")]
    public List<bl_SceneLoaderInfo> sceneList = new List<bl_SceneLoaderInfo>();
    public List<string> tipsList = new List<string>();
    /// <summary>
    /// In case a scene is not setup in the SceneLoaderManager
    /// Try to find the scene in the build settings?
    /// </summary>
    public bool buildSettingsAsFallback = true;
    /// <summary>
    /// In case a scene doesn't have a loading screen, use the default Unity load method? = True
    /// or throw an error and do not load the scene? = False
    /// </summary>
    public bool useSynchronousAsFallback = true;


    /// <summary>
    /// Load an scene using loading screen by the scene Build Index
    /// </summary>
    /// <param name="buildIndex"></param>
    public static void LoadSceneByBuildIndex(int buildIndex)
    {
        if (buildIndex > SceneManager.sceneCountInBuildSettings - 1)
        {
            Debug.LogError($"The scene '{buildIndex}' is not assigned or is disabled in the Build Settings window.");
            return;
        }

        var scene = SceneManager.GetSceneByBuildIndex(buildIndex);
        LoadScene(scene.name);

    }

    /// <summary>
    /// Load an scene using loading screen by the scene Build Index
    /// </summary>
    /// <param name="buildIndex"></param>
    public static void LoadSceneByID(int sceneID)
    {
        if (sceneID > Instance.sceneList.Count - 1)
        {
            Debug.LogError($"The scene '{sceneID}' is not assigned or is disabled in the SceneLoaderManager");
            return;
        }

        var scene = Instance.sceneList[sceneID].SceneName;
        LoadScene(scene);
    }

    /// <summary>
    /// Load an scene using loading screen
    /// </summary>
    /// <param name="sceneName">The scene name</param>
    public static void LoadScene(string sceneName)
    {
        // get the active scene loader in the active scene
        var loadingScreen = FindObjectOfType<bl_SceneLoader>();
        if (loadingScreen == null)
        {
            if (Instance.useSynchronousAsFallback)
            {
                Debug.LogWarning("Don't have any scene loader in this scene, synchronous load method will be used instead.");
                if (Application.CanStreamedLevelBeLoaded(sceneName))
                {
                    Debug.LogWarning($"The scene '{sceneName}' is not set up in the SceneLoaderManager, the scene will be loaded with empty info.");
                    SceneManager.LoadScene(sceneName);
                }
                else
                {
                    Debug.LogError($"The scene '{sceneName}' has not been added or is disabled in the Project Build Settings.");
                }
                return;
            }

            Debug.LogWarning("Don't have any scene loader in this scene.");
            return;
        }

        var sceneInfo = Instance.GetSceneInfo(sceneName);

        if (sceneInfo == null)
        {
            if (Instance.buildSettingsAsFallback)
            {
                if (Application.CanStreamedLevelBeLoaded(sceneName))
                {
                    Debug.LogWarning($"The scene '{sceneName}' is not set up in the SceneLoaderManager, the scene will be loaded with empty info.");
                    sceneInfo = new bl_SceneLoaderInfo();
                    sceneInfo.SceneName = sceneName;
                    sceneInfo.DisplayName = sceneName;
                    sceneInfo.LoadingType = LoadingType.Async;
                }
            }
        }

        if (sceneInfo == null)
        {
            Debug.LogError($"The scene '{sceneName}' could not be found, please check the documentation section 'Add New Scene'");
        }

        loadingScreen.LoadLevel(sceneInfo);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    public bl_SceneLoaderInfo GetSceneInfo(string scene)
    {
        foreach (bl_SceneLoaderInfo info in sceneList)
        {
            if (info.SceneName == scene)
            {
                return info;
            }
        }

        Debug.Log($"'{scene}' is not listed in the 'SceneLoaderManager -> Scene List'", Instance);
        return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string[] GetSceneNames()
    {
        return sceneList.Select(x => x.SceneName).ToArray();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static IEnumerator AsyncLoadData()
    {
        if (_instance == null)
        {
            ResourceRequest rr = Resources.LoadAsync("SceneLoaderManager", typeof(bl_SceneLoaderManager));
            while (!rr.isDone) { yield return null; }
            _instance = rr.asset as bl_SceneLoaderManager;
        }
    }

    public bool HasTips => (tipsList != null && tipsList.Count > 0);

    public static bool IsGlobalLoadingScreen() => Instance.loadingScreenMode == LoadingScreenMode.OneForAll;

    public static bl_SceneLoaderManager _instance;
    public static bl_SceneLoaderManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<bl_SceneLoaderManager>("SceneLoaderManager") as bl_SceneLoaderManager;
            }
            return _instance;
        }
    }

    [System.Serializable]
    public enum LoadingScreenMode
    {
        OnePerScene,
        OneForAll
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        for (int i = 0; i < sceneList.Count; i++)
        {
            if (sceneList[i].SceneAsset != null)
            {
                sceneList[i].SceneName = sceneList[i].SceneAsset.name;
            }
        }
    }
#endif
}