using Tenacity.General.SaveLoad.Implementation;
using Tenacity.General.Interactions.Actions;
using Tenacity.General.Interactions;
using UnityEngine;


namespace Tenacity.General.Events.Actions
{
    [CreateAssetMenu(menuName = "Action/Interaction/MarkItemExistenceAction", fileName = "MarkItemExistenceAction", order = 0)]
    public class MarkItemExistenceAction : InteractionAction
    {
        [SerializeField] private bool _itemExistence;


        public override void Execute(Interaction interaction, Collider intruder)
        {
            var locationItem = interaction.GetComponent<LocationItem>();
            if (locationItem != null)
                locationItem.Exists = _itemExistence;
        }
    }
}