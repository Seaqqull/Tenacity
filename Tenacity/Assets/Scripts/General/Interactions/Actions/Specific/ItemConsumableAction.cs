using Tenacity.Properties;
using UnityEngine;


namespace Tenacity.General.Interactions.Actions
{
    [CreateAssetMenu(fileName = "ItemConsumableAction", menuName = "Action/Interaction/Conditional/ItemConsumable", order = 0)]
    public class ItemConsumableAction : ConditionalInteractionAction<Player.Player>
    {
        [SerializeField] private IntegerReference _requiredId;
        [Header("Actions")] 
        [SerializeField] private InteractionAction _onYes;
        [SerializeField] private InteractionAction _onNo;
        [Space]
        [SerializeField] private InteractionAction _onEnter;
        [SerializeField] private InteractionAction _onExit;
        
        protected override bool IsActionPerformable(Player.Player entity)
        {
            return entity.HasItem(_requiredId.Value);
        }

        protected override void YesAction(Interaction interaction, Collider intruder)
        {
            _onYes?.Execute(Interaction, Intruder);
            // entity.RemoveItem((item) => item.Id == _requiredId);
            //
            // Debug.LogError("Yes action");
        }

        protected override void NoAction(Interaction interaction, Collider intruder)
        {
            _onNo?.Execute(Interaction, Intruder);
        }

        
        public override void OnExit(Interaction interaction, Collider intruder)
        {
            _onExit?.Execute(interaction, intruder);
        }

        public override void OnEnter(Interaction interaction, Collider intruder)
        {
            _onEnter?.Execute(interaction, intruder);
        }
    }
}