using Tenacity.Battles.Views;
using UnityEngine;


namespace Tenacity.Battles.Generators.Battles.Generators.Interactions
{
    public abstract class InteractionFabricSO : ScriptableObject
    {
        public abstract InteractionView CreateInteraction();
    }
}