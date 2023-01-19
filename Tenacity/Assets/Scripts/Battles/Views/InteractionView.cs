using Tenacity.Base;
using Tenacity.Battles.Data;
using UnityEngine;


namespace Tenacity.Battles.Views
{
    [System.Serializable]
    public struct InteractionObject
    {
        public InteractionState State;
        public GameObject Object;
    }
    
    public class InteractionView : BaseMono, IInteractionView
    {
        [SerializeField] private InteractionState _state;
        [SerializeField] private InteractionObject[] _interactions; 
        
        public InteractionState State
        {
            get => _state;
            set
            {
                _state = value;

                foreach (var interaction in _interactions)
                {
                    var shouldBeActive = (interaction.State == _state);
                    if (shouldBeActive != interaction.Object.activeInHierarchy)
                        interaction.Object.SetActive(shouldBeActive);
                }
            }
        }
    }
}