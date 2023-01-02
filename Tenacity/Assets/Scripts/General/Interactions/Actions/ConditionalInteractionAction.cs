using UnityEngine;


namespace Tenacity.General.Interactions.Actions
{
    public abstract class ConditionalInteractionAction<T> : InteractionAction
    {
        protected Interaction Interaction { get; private set; }
        protected Collider Intruder { get; private set; }


        protected abstract bool IsActionPerformable(T entity);
        protected abstract void YesAction(Interaction interaction, Collider intruder);
        protected abstract void NoAction(Interaction interaction, Collider intruder);
        
        
        public override void Execute(Interaction interaction, Collider intruder)
        {
            T entity = intruder.GetComponent<T>();
            if (entity == null) return;

            
            Interaction = interaction;
            Intruder = intruder;
            
            if (IsActionPerformable(entity))
                YesAction(interaction, intruder);
            else
                NoAction(interaction, intruder);
        }
    }
}