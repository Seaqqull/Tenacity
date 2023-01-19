using Tenacity.Battles.Views.Creatures;
using Tenacity.Battles.Data.Field;
using Tenacity.Battles.Lands.Data;
using System.Linq;
using UnityEngine;


namespace Tenacity.Battles.Generators.Creatures
{
    [System.Serializable]
    public class PlayerInstance
    {
        public TeamType Team;
        public CreatureView Prefab;
    }
    [System.Serializable]
    public class CreatureInstance
    {
        public TeamType Team;
        public LandType Type;
        public CreatureView Prefab;
    }

    [CreateAssetMenu(fileName = "OrdinalCreatureFabric", menuName = "Battles/Creatures/OrdinalCreatureFabric")]
    public class OrdinalCreatureFabric : CreatureFabricSO
    {
        [SerializeField] private PlayerInstance[] _players;
        [Space]
        [SerializeField] private CreatureInstance[] _creatures;
        
        
        public override CreatureView CreatePlayerCreature(TeamType team)
        {
            var player = _players.FirstOrDefault(player => (player.Team == team));
            if (player == null)
                Debug.LogError($"[OrdinalCreatureFabric] Error: Player of team({team}) was not found.", this);
            return (player == null) ? null : GameObject.Instantiate<CreatureView>(player.Prefab);
        }

        public override CreatureView CreateCardCreature(TeamType team, LandType type)
        {
            var creature = _creatures.FirstOrDefault(creature => creature.Team.HasFlag(team) && creature.Type.HasFlag(type));
            if (creature == null)
                Debug.LogError($"[OrdinalCreatureFabric] Error: Creature of team({team}) & type({type}) was not found.", this);
            return (creature == null) ? null : Instantiate(creature.Prefab, Vector3.zero, Quaternion.identity);
        }
    }
}