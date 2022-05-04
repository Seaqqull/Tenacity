using UnityEngine;


namespace Tenacity.Cards
{
    [CreateAssetMenu(fileName = "Battle Character Template", menuName = "Battle/BattleCharacter")]
    public class BattleCharacterSO : CardDataSO
    {
        [SerializeField] private GameObject _characterPrefab;
        //...

        public GameObject CharacterPrefab => _characterPrefab;
    }
}