using Tenacity.Cards;
using UnityEngine;


namespace Tenacity.Battles.Controllers
{
    public class CreatureController : MonoBehaviour
    {
        private CreatureDragging _creatureDragging;
        private CardItem _cardCreature;


        private void Awake()
        {
            _cardCreature = GetComponent<CardItem>();
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
        
        
        public void Attack(CardItem cardToAttack)
        {
            if (cardToAttack == null) return;

            int figthBackPower = cardToAttack.Data.Power;
            cardToAttack.GetDamaged(_cardCreature.Data.Power);
            GetComponent<CardItem>().GetDamaged(figthBackPower);
            _cardCreature.enabled = false;
        }
    }
}