using UnityEngine;


namespace Tenacity.General.Interactions
{
    public class GraphicalInteraction : ConditionalInteraction
    {
        [Header("Ui")]
        [SerializeField] private GameObject _objectToShow;
        [SerializeField] private KeyCode _interactionButton; // Will be changed to some click action


        protected override void Start()
        {
            base.Start();
            
            NeedToShow(_isInteractable);
        }

        public override void OnTriggerEnter(Collider collision)
        {
            base.OnTriggerEnter(collision);

            NeedToShow(true);
        }

        public override void OnTriggerExit(Collider collision)
        {
            base.OnTriggerExit(collision);

            NeedToShow(false);
        }

        protected virtual void FixedUpdate()
        {
            if ((_action != null) && _isInteractable && Input.GetKeyDown(_interactionButton))
                _action.Execute(this, _collision);
        }


        protected virtual void NeedToShow(bool flag)
        {
            if (_objectToShow == null) return;

            _objectToShow.SetActive(flag);
        }
    }
}