using System.Collections;
using UnityEngine;

namespace Tenacity.Battle
{
    [CreateAssetMenu(fileName = "Battle Player Template", menuName = "Battle/BattlePlayer")]
    public class BattlePlayer : ScriptableObject
    {

        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private int _health;


        public int Health => _health;
        public GameObject PlayerPrefab => _playerPrefab;
    }
}