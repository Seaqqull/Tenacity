using UnityEngine;


namespace Tenacity.Battles
{
    [CreateAssetMenu(fileName = "Battle Enemy Template", menuName = "Battle/BattleEnemy")]
    public class BattleEnemy : ScriptableObject
    {
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private int _health;
    }
}