using UnityEngine.Events;
using UnityEngine;


namespace Tenacity.Dummy
{
    [RequireComponent(typeof(Collider2D))]
    public class ColliderListener : MonoBehaviour
    {
        [System.Serializable]
        public class ColliderInteraction : UnityEvent<Collider2D> { }
        
        [field: SerializeField] public Collider2D Collider { get; private set; }
        [field: SerializeField] public bool InInteraction { get; private set; }
        [SerializeField] private ColliderInteraction _onEnter;
        [SerializeField] private ColliderInteraction _onExit;
        
        public event UnityAction<Collider2D> OnEnter
        {
            add { _onEnter.AddListener(value); } 
            remove { _onEnter.AddListener(value); }
        }
        public event UnityAction<Collider2D> OnExit
        {
            add { _onExit.AddListener(value); } 
            remove { _onExit.AddListener(value); }
        }


        private void Awake()
        {
            if (Collider == null)
                Collider = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            InvokeEnter(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            InvokeExit(other);
        }


        public void InvokeExit()
        {
            InvokeExit(null);
        }
        
        public void InvokeEnter()
        {
            InvokeEnter(null);
        }

        public void InvokeExit(Collider2D other)
        {
            if (!InInteraction)
                return;

            _onExit?.Invoke(other);
            InInteraction = false;
        }
        
        public void InvokeEnter(Collider2D other)
        {
            if (InInteraction)
                return;
            
            _onEnter?.Invoke(other);
            InInteraction = true;
        }
    }
}