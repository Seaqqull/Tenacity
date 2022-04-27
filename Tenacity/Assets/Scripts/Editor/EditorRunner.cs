using UnityEngine;


namespace Tenacity.Editor
{
    public class EditorRunner : MonoBehaviour
    {
        [SerializeField] [Range(1, 120)] private int targetFramerate = 60;
        [SerializeField] [Range(0, 4)] private int vSyncCount = 1;
        
#if UNITY_EDITOR       
        private void Start()
        {
            Application.targetFrameRate = targetFramerate;
            QualitySettings.vSyncCount = vSyncCount;
        }
#endif
    }
}