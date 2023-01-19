using Tenacity.Battles.Data.Field;
using UnityEngine;


namespace Tenacity.Battles.Controllers.CellModifiers
{
    [CreateAssetMenu(fileName = "PlacementRotationModifier", menuName = "Battles/CellModifier/PlacementRotationModifier")]
    public class PlacementRotationModifierSO : CellModifierSO
    {
        [SerializeField] private Vector3 _topTeamRotation;
        [SerializeField] private Vector3 _bottomTeamRotation;
        
        
        public override void ModifyCell(CellController cell)
        {
            var state = cell.State;
            cell.PlacementParent.rotation = (state.Team == TeamType.Top) ? Quaternion.Euler(_topTeamRotation) :
                (state.Team == TeamType.Bottom) ? Quaternion.Euler(_bottomTeamRotation) : Quaternion.identity;
        }
    }
}