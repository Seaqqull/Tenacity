using System.Collections;
using Tenacity.Battle;
using UnityEngine;

namespace Tenacity.Cards
{
    public class CreatureController : MonoBehaviour
    {

        private Card _cardCreature;
        private CreatureDragging _creatureDragging;


        private void Awake()
        {
            _cardCreature = GetComponent<Card>();
            _cardCreature.enabled = false;
            if (GetComponentInParent<CreatureDragging>())
            {
               _creatureDragging = transform.root.GetComponentInChildren<CreatureDragging>();
            }
        }

        public void Attack(Card cardToAttack)
        {
            if (cardToAttack == null) return;

            int figthBackPower = cardToAttack.Data.Power;
            cardToAttack.GetDamaged(_cardCreature.Data.Power);
            GetComponent<Card>().GetDamaged(figthBackPower);
            _cardCreature.enabled = false;
        }

        private void OnMouseDown()
        {
            if (!_creatureDragging.IsPlayerTurn) return;
            if (_creatureDragging.IsCurrentlyMovingCreature) return;
            if (!_creatureDragging.enabled || !_cardCreature.enabled) return;

            _creatureDragging.SelectCreature(_cardCreature);
        }
    }
}