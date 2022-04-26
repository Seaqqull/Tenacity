using System.Collections;
using UnityEngine;

namespace Tenacity.Cards
{
    public class CardController : MonoBehaviour
    {
        private Card _card;

        private void Awake()
        {
            _card = GetComponent<Card>();
        }

        public void Attack(Card cardToAttack)
        {
            if (cardToAttack == null) return;

            int figthBackPower = cardToAttack.Data.Power;
            cardToAttack.GetDamaged(_card.Data.Power);
            _card.GetDamaged(figthBackPower);
            _card.enabled = false;
        }
    }
}