using UnityEngine.SceneManagement;
using Tenacity.Properties;
using Tenacity.UI.Menus;
using Tenacity.Base;
using UnityEngine;


namespace Tenacity.Managers
{
    public class StartupInitializer : SingleBehaviour<StartupInitializer>
    {
        [Space]
        [SerializeField] private bool _singleOnly = true;
        [SerializeField] private IntegerVariable _preloadSceneIndex;
        [Space] 
        [SerializeField] private IntegerVariable _currentSceneIndex;
        
        
        protected override void Awake()
        {
            base.Awake();
            
            if ((Instance == this) || !_singleOnly)
                UnityEngine.SceneManagement.SceneManager.LoadScene(_preloadSceneIndex.Value, LoadSceneMode.Additive);
        }

        private void Start()
        {
            SceneManager.Instance.UpdateLevelIndex(_currentSceneIndex.Value);
            SettingsMenu.SetFrameRate();
        }
    }
}
