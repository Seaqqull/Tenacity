using UnityEngine;


namespace Tenacity.General.Items
{
    public class PickableItem : Item
    {
        [SerializeField] protected Interactions.Interaction _interaction;


        protected override void Awake()
        {
            base.Awake();

            if (_interaction == null)
                _interaction = GetComponentInChildren<Interactions.Interaction>();
        }


        public override bool SetPickable(bool flag)
        {
            if (_interaction == null) return false;

            base.SetPickable(flag);

            _interaction.SetActive(flag);        
            return true;
        }
    }
}