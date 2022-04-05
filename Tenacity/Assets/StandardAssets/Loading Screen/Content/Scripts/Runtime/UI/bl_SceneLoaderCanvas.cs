using UnityEngine;

namespace Lovatto.SceneLoader
{
    public class bl_SceneLoaderCanvas : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        private void Awake()
        {
            if (bl_SceneLoaderManager.IsGlobalLoadingScreen())
            {
                var sl = FindObjectOfType<bl_SceneLoaderCanvas>();
                if (sl != null && sl != this)
                {
                    gameObject.SetActive(false);
                    return;
                }

                DontDestroyOnLoad(gameObject);
            }
        }
    }
}