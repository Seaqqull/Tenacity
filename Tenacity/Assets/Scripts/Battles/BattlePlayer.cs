using UnityEngine;


namespace Tenacity.Battles
{
    [CreateAssetMenu(fileName = "Battle Player Template", menuName = "Battle/BattlePlayer")]
    public class BattlePlayer : ScriptableObject
    {

        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private int _health;
        
        public GameObject PlayerPrefab => _playerPrefab;
        public int Health => _health;
    }
}