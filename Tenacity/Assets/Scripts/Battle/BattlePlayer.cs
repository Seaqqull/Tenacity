using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Battle
{
    public class BattlePlayer : MonoBehaviour
    {
        [SerializeField] private GameObject _cardDeck;
        [SerializeField] private int _currentMana;

        public int CurrentMana
        {
            get => _currentMana;
            set => _currentMana = value;
        }
    }
}