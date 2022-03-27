using UnityEngine;


namespace Tenacity.General.Interactions
{
    public class ConditionalInteraction : Interaction
    {
        protected bool _isInteractable;
        protected Collider _collision;


        public override void OnTriggerEnter(Collider collision)
        {
            _isInteractable = true;
            _collision = collision;

            _action.OnEnter(this, collision);
        }

        public virtual void OnTriggerExit(Collider collision)
        {
            _isInteractable = false;
            _collision = null;

            _action.OnExit(this, collision);
        }
    }
}