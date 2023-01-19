using Tenacity.Battles.Data;
using UnityEngine;
using TMPro;


namespace Tenacity.Battles.Views.Players
{
    public sealed class UiPlayerView : PlayerView
    {
        [Space]
        [SerializeField] private MaskableValueView _health;
        [SerializeField] private MaskableValueView _mana;
        [Space] 
        [SerializeField] private TMP_Text _name;
        [SerializeField] private GameObject _ground;

        
        public override void UpdateData(PlayerDataView data)
        {
            base.UpdateData(data);
            
            OnHealthUpdate(Data.Health, Data.MaxHealth);
            OnManaUpdate(Data.Mana, Data.MaxMana);
            SetGroundAvailability(Data.GroundEnabled);
            SetName(Data.Name);
        }
        
        
        private void SetName(string name)
        {
            _name.text = name;
        }

        private void SetGroundAvailability(bool isActive)
        {
            _ground.SetActive(isActive);
        }
        
        private void OnManaUpdate(int amount, int maxAmount)
        {
            _mana.FillAmount(amount, maxAmount);
        }

        private void OnHealthUpdate(int amount, int maxAmount)
        {
            _health.FillAmount(amount, maxAmount);
        }
    }
}