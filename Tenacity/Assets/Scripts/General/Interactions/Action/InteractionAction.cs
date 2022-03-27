using UnityEngine;


namespace Tenacity.General.Interactions.Action
{
    public abstract class InteractionAction : ScriptableObject
    {
        [SerializeField] private float _delay;

        public float Delay
        {
            get { return this._delay; }
        }

        
        public virtual void OnEnter(Interaction interaction, Collider intruder) { }
        
        public virtual void OnExit(Interaction interaction, Collider intruder) { }
        

        public abstract void Execute(Interaction interaction, Collider intruder);
    }
}