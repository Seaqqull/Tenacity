using UnityEngine;


namespace Tenacity.Utility.Base
{
    public class SingleBehaviour<T> : MonoBehaviour where T : SingleBehaviour<T>
    {
        [Header("Specific")] 
        [SerializeField] private bool _rewritable;
        [SerializeField] private bool _dontDestroyOnLoad;
        
        public static T Instance { get; private set; }


        protected virtual void Awake()
        {
            if (!_rewritable && Instance != null)
            {
                Destroy(this);
                return;
            }
            
            if (Instance != null)
                Destroy(Instance);
            if(_dontDestroyOnLoad)
                DontDestroyOnLoad((this as T));
            Instance = (T)this;
        }
    }
}