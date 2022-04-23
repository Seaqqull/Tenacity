using UnityEngine;

namespace Tenacity.Battle
{
    public class BattleEnemy : MonoBehaviour
    {
        [SerializeField] private int health;

        public int CurrentMana
        {
            get => _currentMana;
            set => _currentMana = value;
        }

        private int _currentMana;
    }
}