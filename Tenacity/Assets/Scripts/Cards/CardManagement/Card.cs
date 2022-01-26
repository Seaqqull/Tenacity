using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Tenacity.Cards
{
    public class Card : MonoBehaviour
    {
        [SerializeField]
        private CardFactory _cardFactory;

        private CardTemplate card;

        public CardFactory CardFactory { get => _cardFactory; }
        public List<Component> CardComponents { get; private set; }
        public List<Component> CardProperties { get; private set; }

        public static string PropertyFactoryName = nameof(_cardFactory);

        private void Awake()
        {
             card = _cardFactory.InitDefaultCard();
             CardComponents = new List<Component>(GetComponentsInChildren<Component>());
        }

        public void UpdatePropertiesList()
        {
            card = CardFactory.GetCurrentCardClass();
            UpdateCardView();
            //var standardProps = card.GetType().GetProperties();
        }

        private void UpdateCardView()
        {
            //GameObject.Find("ID").GetComponent<TextMeshPro>().text = card.CardID.ToString();
            GameObject.Find("Name").GetComponent<TextMeshPro>().text = card.CardName.ToString();
            GameObject.Find("Cost").GetComponent<TextMeshPro>().text = card.CardCost.ToString();
           
            var type = CardFactory.Type;
            if (type == CardFactory.CardType.AttackCard) {
                SetChildActive("CardPower", true);
                SetChildActive("CardLife", false);
                GameObject.Find("Power").GetComponent<TextMeshPro>().text = ((AttackCard)card).Attack.ToString();
            } else if (type == CardFactory.CardType.DefenseCard) {
                SetChildActive("CardPower", false);
                SetChildActive("CardLife", true);
                GameObject.Find("Life").GetComponent<TextMeshPro>().text = ((DefenseCard)card).Defense.ToString();
            }  else if (type == CardFactory.CardType.Standard || type == CardFactory.CardType.None){
                SetChildActive("CardPower", false);
                SetChildActive("CardLife", false);
            } else {
                SetChildActive("CardPower", true);
                SetChildActive("CardLife", true);
                GameObject.Find("Power").GetComponent<TextMeshPro>().text = ((CombinedCard)card).Attack.ToString();
                GameObject.Find("Life").GetComponent<TextMeshPro>().text = ((CombinedCard)card).Defense.ToString();
            }
            GameObject.Find("Description").GetComponent<TextMeshPro>().text = card.CardDescription;
            GameObject.Find("Creature").GetComponent<SpriteRenderer>().sprite = card.Creature;
        }

        private void SetChildActive(string propName, bool active)
        {
            transform.Find(propName).gameObject.SetActive(active);
        }
        /*
        private void SetTextMeshProValue(string propName, string value)
        {
            GameObject.Find(propName).GetComponent<TextMeshPro>().text = card.;
        }
        */
    }
}