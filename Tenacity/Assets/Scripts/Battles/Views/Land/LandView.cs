using Tenacity.Battles.Lands.Data;
using Tenacity.Battles.Data;
using Tenacity.Battles.Views.Players;
using UnityEngine;


namespace Tenacity.Battles.Views.Land
{
    public class LandView : PlayerView
    {
        [SerializeField] private SingleLandView[] _lands;
        
        private LandType _selectedLand = LandType.None;


        private void Awake()
        {
            foreach (var land in _lands)
                land.OnClick += OnLandSelection;
        }


        private void OnLandSelection(LandType landType)
        {
            var landSelected = Data.SelectLand(Data.PlayerId, landType);

            _selectedLand = landSelected ? landType : LandType.None;
            foreach (var land in _lands)
                land.Selected = landSelected && (land.Land == _selectedLand);
        }
        

        public override void UpdateData(PlayerDataView data)
        {
            base.UpdateData(data);
            // if (!Data.HideLandSelection)
            //     return;
            
            _selectedLand = LandType.None;
            foreach (var land in _lands)
                land.Selected = false;
        }
    }
}