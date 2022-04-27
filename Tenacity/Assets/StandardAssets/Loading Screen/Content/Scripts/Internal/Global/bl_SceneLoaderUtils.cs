using UnityEngine;
using Lovatto.SceneLoader;
#if UNITY_5_3|| UNITY_5_4 || UNITY_5_3_OR_NEWER || UNITY_2017
using UnityEngine.SceneManagement;
#endif

public static class bl_SceneLoaderUtils
{
    /// <summary>
    /// 
    /// </summary>
    public static bl_SceneLoader GetLoader
    {
        get
        {
            bl_SceneLoader sl = GameObject.FindObjectOfType<bl_SceneLoader>();
            if(sl == null)
            {
                Debug.LogWarning("Don't have any scene loader in this scene.");
            }
            return sl;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static bl_SceneLoaderManager GetSceneLoaderManager()
    {
        bl_SceneLoaderManager slm = Resources.Load("SceneLoaderManager", typeof(bl_SceneLoaderManager)) as bl_SceneLoaderManager;
        if(slm == null)
        {
            Debug.LogWarning("Not found any scene loader manager in resources folder!");
        }
        return slm;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="scene"></param>
    public static AsyncOperation LoadLevelAsync(int scene)
    {
#if UNITY_5_3 || UNITY_5_4|| UNITY_5_3_OR_NEWER || UNITY_2017
       return SceneManager.LoadSceneAsync(scene);
#else
        return Application.LoadLevelAsync(scene);
#endif
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="scene"></param>
    public static AsyncOperation LoadLevelAsync(string scene)
    {
#if UNITY_5_3 || UNITY_5_4|| UNITY_5_3_OR_NEWER || UNITY_2017
        return  SceneManager.LoadSceneAsync(scene);
#else
        return Application.LoadLevelAsync(scene);
#endif
    }

}