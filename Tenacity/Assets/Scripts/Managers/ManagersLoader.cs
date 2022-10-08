using UnityEngine.SceneManagement;
using Tenacity.Properties;
using Tenacity.Base;
using UnityEngine;


namespace Tenacity.Managers
{
    public class ManagersLoader : SingleBehaviour<ManagersLoader>
    {
        [Space]
        [SerializeField] private bool _singleOnly = true;
        [SerializeField] private IntegerVariable _sceneIndex;
        [Space] 
        [SerializeField] private IntegerVariable _currentSceneIndex;
        
        
        protected override void Awake()
        {
            base.Awake();
            
            if ((Instance == this) || !_singleOnly)
                UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneIndex.Value, LoadSceneMode.Additive);
        }

        private void Start()
        {
            SceneManager.Instance.UpdateLevelIndex(_currentSceneIndex.Value);
        }
    }
}
