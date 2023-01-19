using System.Collections.Generic;
using System.Linq;
using Tenacity.Battles.Data;
using Tenacity.Battles.Views.Players;
using UnityEngine;


namespace Tenacity.Battles.Views.Cards
{
    public class HandView : PlayerView
    {
        #region Constants
        private const int NO_SELECTION = -1;
        #endregion
        
        
        [SerializeField] private CardView _cardPrefab;
        [SerializeField] private Transform[] _slots;

        private List<CardView> _cards = new ();

        private int _selectedCardIndex = NO_SELECTION;
        // Reset selection
        // Select
        // Deselect = Reset selection?

        private void OnCardSelection(int cardIndex)
        {
            var cardSelected = Data.SelectCard(Data.PlayerId, cardIndex);

            _selectedCardIndex = cardSelected ? cardIndex : NO_SELECTION;
            for (int i = 0; i < _cards.Count; i++)
                _cards[i].Selected = (i == _selectedCardIndex);
        }
        

        public override void UpdateData(PlayerDataView data)
        {
            base.UpdateData(data);

            foreach (var card in _cards)
                Destroy(card.GameObject);
            _cards.Clear();

            var cards = data.Hand.Cards.ToArray();
            for (int i = 0; (i < cards.Length) && (i < _slots.Length); i++)
            {
                var newCard = Instantiate(_cardPrefab, Vector3.zero, Quaternion.identity);
                newCard.Transform.SetParent(_slots[i]);
                newCard.Transform.localPosition = Vector3.zero;
                newCard.Transform.localRotation = Quaternion.identity;

                var handCardIndex = i;
                newCard.OnClick += () => OnCardSelection(handCardIndex);
                newCard.FillData(cards[i]);
                
                _cards.Add(newCard);
            }
        }
    }
}