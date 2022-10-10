using UnityEngine;


namespace Tenacity.Managers
{
    public class GameStorageUpdateManager : Base.SingleBehaviour<GameStorageUpdateManager>
    {
        private static void UpdateFrameRate(int targetFramerate)
        {
            QualitySettings.vSyncCount = Application.isMobilePlatform ? 0 : 1; // 1 for Standalone - if component is hidden on that platform 
            if (Application.isMobilePlatform)
            {
                Application.targetFrameRate = targetFramerate;
            }
#if UNITY_EDITOR
            else
            {
                Application.targetFrameRate = targetFramerate;
            }
#endif
        }
        
        
        public void UpdateGame(StorageManager storage)
        {
            UpdateFrameRate(storage.TargetFramerate);
        }
    }
}