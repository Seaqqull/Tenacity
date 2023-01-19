using Tenacity.Battles.Views.Creatures;
using Tenacity.Battles.Data.Field;
using Tenacity.Battles.Lands.Data;
using UnityEngine;


namespace Tenacity.Battles.Generators.Creatures
{
    public abstract class CreatureFabricSO : ScriptableObject
    {
        public abstract CreatureView CreatePlayerCreature(TeamType team);
        public abstract CreatureView CreateCardCreature(TeamType team, LandType type);
    }
}