using UnityEngine;

namespace Tenacity.Cards {

    [CreateAssetMenu(fileName = "CardScriptableObject", menuName = "ScriptableObjects/Card")]
    public class CardScriptableObject : ScriptableObject
    {
        [SerializeField]
        public CardTemplate card;

        private void InitCardSO(CardTemplate _card)
        {
            card = _card;
        }

        public static void CreateCardSO(CardTemplate _card)
        {
            var cardObject = CreateInstance<CardScriptableObject>();
            cardObject.InitCardSO(_card);
        }
    }

}
