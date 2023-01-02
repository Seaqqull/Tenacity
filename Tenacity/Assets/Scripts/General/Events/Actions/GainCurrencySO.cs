using Tenacity.General.Items.Consumables;
using UnityEngine;


namespace Tenacity.General.Events.Actions
{
    [CreateAssetMenu(menuName = "Events/Actions/GainCurrency", fileName = "GainCurrency")]
    public class GainCurrencySO : ActionSO<bool>
    {
        [SerializeField] private CoinSO _coin;
        
        
        protected override bool PerformAction()
        {
            return true;
        }

        public override bool Perform()
        {
            return true;
        }


        public void GainCurrency()
        {
            var player = FindObjectOfType<Player.Player>();
            if (player != null)
                player.Consume(_coin);
        }
    }
}