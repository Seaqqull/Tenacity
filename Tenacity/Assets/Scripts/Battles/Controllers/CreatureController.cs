using Tenacity.Cards;
using UnityEngine;


namespace Tenacity.Battles.Controllers
{
    public class CreatureController : MonoBehaviour
    {
        private CreatureDragging _creatureDragging;
        private Card _cardCreature;


        private void Awake()
        {
            _cardCreature = GetComponent<Card>();
            _cardCreature.enabled = false;
            
            if (GetComponentInParent<CreatureDragging>() != null)
               _creatureDragging = transform.root.GetComponentInChildren<CreatureDragging>();
        }

        
        private void OnMouseDown()
        {
            if (!_creatureDragging.IsPlayerTurn || _creatureDragging.IsCurrentlyMovingCreature || 
                !_creatureDragging.enabled || !_cardCreature.enabled) return;

            _creatureDragging.SelectCreature(_cardCreature);
        }
        
        
        public void Attack(Card cardToAttack)
        {
            if (cardToAttack == null) return;

            int figthBackPower = cardToAttack.Data.Power;
            cardToAttack.GetDamaged(_cardCreature.Data.Power);
            GetComponent<Card>().GetDamaged(figthBackPower);
            _cardCreature.enabled = false;
        }
    }
}