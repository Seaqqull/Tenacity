using MainPlayer = Tenacity.Player.Player;
using Tenacity.Managers;
using UnityEngine;
using TMPro;


namespace Tenacity.UI.Menus
{
    public class InventoryMenu : SingleMenu<InventoryMenu>
    {
        #region Constants
        private const int SILVER_CURRENCY_RATIO = 100;
        private const int GOLD_CURRENCY_RATIO = 10000;
        #endregion
        
        [SerializeField] private TMP_Text _goldText;
        [SerializeField] private TMP_Text _silverText;
        [SerializeField] private TMP_Text _bronzeText;
        
        
        protected override void Awake()
        {
            base.Awake();

            var player = PlayerManager.Instance.GetComponent<MainPlayer>();
            var currency = player.Currency;

            HandleCurrency(currency);
        }


        private void HandleCurrency(int currency)
        {
            var gold = currency / GOLD_CURRENCY_RATIO;
            currency -= (gold * GOLD_CURRENCY_RATIO);
            var silver = currency / SILVER_CURRENCY_RATIO;
            currency -= (silver * SILVER_CURRENCY_RATIO);
            var bronze = currency % SILVER_CURRENCY_RATIO;
            
            _goldText.text = gold.ToString();
            _silverText.text = silver.ToString();
            _bronzeText.text = bronze.ToString();
        }
    }
}
