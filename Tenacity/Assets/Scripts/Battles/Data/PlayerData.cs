using Tenacity.Battles.Data.Field;
using Tenacity.Battles.Lands.Data;
using System;


namespace Tenacity.Battles.Data
{
    public interface IPlayerView
    {
        public void PerformDamage(int amount);
        public void UpdateData(PlayerDataView data);
    }

    public interface IPlayer : IPlayerData
    {
        public IPlayerView View { get; set; }
        public TeamType Team { get; }
        public bool IsDead { get; }
        public int Id { get; }

        
        public void Heal(int amount);
        public void PerformDamage(int amount);
    }
    
    public interface IPlayerData
    {
        public ICardDeck Hand { get; }
        public ICardDeck Deck { get; }

        public int MaxHealth { get; }
        public int HandSize { get; }
        public string Name { get; }
        public int Health { get; }
        public int Mana { get; }
        
        public int Seed { get; }

        public void UpdateDeck(ICardDeck deck);
    }

    public class PlayerData : IPlayerData
    {
        public ICardDeck Hand { get; set; }
        public ICardDeck Deck { get; set; }
        public int MaxHealth { get; set; }
        public int HandSize { get; set; }
        public string Name { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }

        public int Seed { get; set; }
        
        
        public void UpdateDeck(ICardDeck deck) => Deck = deck;
    }
    
    public class PlayerDataView
    {
        public int PlayerId { get; set; }

        public ICardDeck Hand { get; set; }
        public int MaxHealth { get; set; }
        public int MaxMana { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }

        public bool GroundEnabled { get; set; }
        public string Name { get; set; }
        
        public Func<int, LandType, bool> SelectLand { get; set; }
        public Func<int, int, bool> SelectCard { get; set; }
        // public bool HideLandSelection { get; set; }
        // public bool HideCardSelection { get; set; }


        public PlayerDataView(PlayerDataView player)
        {
            MaxHealth = player.MaxHealth;
            PlayerId = player.PlayerId;
            MaxMana = player.MaxMana;
            Health = player.Health;
            Mana = player.Mana;
            
            GroundEnabled = player.GroundEnabled;
            Hand = player.Hand;
            Name = player.Name;
            
            // HideLandSelection = player.HideLandSelection;
            // HideCardSelection = player.HideCardSelection;
            SelectLand = player.SelectLand;
            SelectCard = player.SelectCard;
        }
        
        public PlayerDataView() { }
    }
}
