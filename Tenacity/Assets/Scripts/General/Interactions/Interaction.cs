using Tenacity.General.Items;
using Tenacity.Base;
using UnityEngine;


namespace Tenacity.General.Interactions
{
    public class Interaction : BaseMono
    {
        [SerializeField] protected Item _itemToInteract;
        [SerializeField] protected GameObject _owner;
        [SerializeField] protected Actions.InteractionAction _action;
        
        protected Sequence.SequentialExecutor[] _modes;
        
        public Item ObjectToInteract
        {
            get { return _itemToInteract; }
        }
        public virtual bool IsActive
        {
            get { return gameObject.activeSelf; }
        }
        public GameObject Owner
        {
            get { return this._owner; }
        }



        protected override void Awake()
        {
            base.Awake();

            _modes = GetComponentsInChildren<Sequence.SequentialExecutor>();
        }

        protected virtual void Start()
        {
            if (_action == null)
                Debug.LogError("There is no action attached.", GameObject);
        }


        public virtual void OnTriggerEnter(Collider collision)
        {
            if (_action != null)
                _action.Execute(this, collision);         
        }


        public void SetActive(bool flag)
        {
            gameObject.SetActive(flag);
        }

        public void SetModesActive(bool flag)
        {
            for (int i = 0; i < _modes.Length; i++)
                _modes[i].enabled = flag;
        }
        
        public virtual void Interact(Collider collision = null)
        {
            if (IsActive)
                _action.Execute(this, collision);
        }
        
        public virtual void InteractAnyway(Collider collision = null)
        {
            _action.Execute(this, collision);
        }
    }
}