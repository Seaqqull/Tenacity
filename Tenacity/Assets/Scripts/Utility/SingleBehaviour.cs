using UnityEngine;


namespace Tenacit.Utility.Constants
{
    public class SingleBehaviour<T> : MonoBehaviour where T : SingleBehaviour<T>
    {
        [Header("Rewritable")] 
        [SerializeField] private bool _rewritable;
        
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
            Instance = (T)this;
        }
    }
}