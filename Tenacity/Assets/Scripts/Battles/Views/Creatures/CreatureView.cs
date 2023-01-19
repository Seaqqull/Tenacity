using Tenacity.Battles.Data.Field;
using Tenacity.Battles.Lands.Data;
using Tenacity.Battles.Data;
using Tenacity.Base;
using UnityEngine;
using System;
using TMPro;


namespace Tenacity.Battles.Views.Creatures
{
    public class CreatureView : BaseMono, IPlayerView
    {
        [System.Serializable]
        private struct TeamOutlier
        {
            public TeamType Team;
            public GameObject Object;
        }
        
        [SerializeField] private TMP_Text _powerText;
        [SerializeField] private TMP_Text _lifeText;
        [field: Header("Read-only")]
        [field: SerializeField] public TeamType Team { get; private set; }
        [field: SerializeField] public LandType Type { get; private set; }
        [field: SerializeField] public int Priority { get; private set; }
        [field: SerializeField] public int Power { get; private set; }
        [field: SerializeField] public int Health { get; private set; }
        [Space] 
        [SerializeField] private TeamOutlier[] _outliers;

        public Action<int> OnHealthUpdate { get; private set; }
        public CreatureData Data { get; private set; }


        public void UpdateCreature(CreatureData data)
        {
            var updateFromDamage = data.Life < Health;
            
            OnHealthUpdate = ((data.OnHealthUpdate != null) && (OnHealthUpdate == null)) ? data.OnHealthUpdate : OnHealthUpdate;
            Power = data.Power;
            Health = data.Life;
            Team = data.Team;
            Type = data.Type;
            Data = data;
            
            
            // Update some UI
            if (updateFromDamage)
                OnHealthUpdate?.Invoke(Health);
            _powerText?.SetText(Power.ToString());
            _lifeText?.SetText(Health.ToString());

            foreach (var outlier in _outliers)
            {
                var properTeam = (outlier.Team == Team);
                if (properTeam != outlier.Object.activeSelf)
                    outlier.Object.SetActive(properTeam);
            }
        }

        public void PerformDamage(int amount)
        {
            Health -= amount;
            _lifeText?.SetText(Health.ToString());
            
            OnHealthUpdate?.Invoke(Health);
        }

        public void UpdateData(PlayerDataView data)
        {
            // var updateFromDamage = data.Life < Health;

            Health = data.Health;
        }
    }
}