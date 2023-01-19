using Tenacity.Battles.Views;
using UnityEngine;


namespace Tenacity.Battles.Generators.Battles.Generators.Interactions
{
    [CreateAssetMenu(fileName = "OrdinalInteractionFabric", menuName = "Battles/Field/OrdinalInteractionFabric")]
    public class OrdinalInteractionFabric : InteractionFabricSO
    {
        [SerializeField] private InteractionView _interaction;
        
    
        public override InteractionView CreateInteraction()
        {
            return Instantiate(_interaction, Vector3.zero, Quaternion.identity);
        }
    }
}