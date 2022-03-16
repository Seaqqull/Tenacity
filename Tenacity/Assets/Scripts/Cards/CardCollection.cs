using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tenacity.Cards
{
    public class CardCollection : MonoBehaviour
    {
        [SerializeField] private List<CardData> cards = new List<CardData>();

        public List<CardData> Cards => cards;
    }
}