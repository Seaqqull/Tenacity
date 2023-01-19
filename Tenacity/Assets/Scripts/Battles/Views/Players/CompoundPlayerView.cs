using Tenacity.Battles.Data;
using UnityEngine;


namespace Tenacity.Battles.Views.Players
{
    public class CompoundPlayerView : PlayerView
    {
        [SerializeField] private PlayerView[] _views;


        public override void UpdateData(PlayerDataView data)
        {
            base.UpdateData(data);

            foreach (var view in _views)
            {
                view.UpdateData(Data);
            }
        }
    }
}