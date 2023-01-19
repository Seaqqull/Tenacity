using UnityEngine;


namespace Tenacity.Battles.Data
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Battles/Players/PlayerData", order = 0)]
    public class PlayerDataSO : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public int MaxHealth { get; private set; }
        [field: SerializeField] public int Health { get; private set; }
        [field: SerializeField] public int Mana { get; private set; }
        [field: SerializeField] public int HandSize { get; private set; }


        [field: SerializeField] public bool IsAi { get; private set; }
    }
}