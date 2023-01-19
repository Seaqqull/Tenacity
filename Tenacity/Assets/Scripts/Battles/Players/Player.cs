using Tenacity.Battles.Data.Field;
using Tenacity.Battles.Data;
using UnityEngine;
using System;


namespace Tenacity.Battles.Players
{
    public abstract class Player : IPlayer
    {
        private ICardDeck _deck;
        
        public bool IsDead { get; protected set; }
        public int Health { get; private set; }
        public int Mana { get; private set; }
        public IPlayerView View { get; set; }
        public int Id { get; private set; }
        public ICardDeck Hand { get; private set; }
        public ICardDeck Deck { get => _deck; }
        public TeamType Team { get; }
        public int MaxHealth { get; }
        public int HandSize { get; }
        public string Name { get; }

        public int Seed { get; }


        protected Player(IPlayerData data, int id, TeamType team)
        {
            MaxHealth = data.MaxHealth;
            HandSize = data.HandSize;
            Health = data.Health;
            Name = data.Name;
            Mana = data.Mana;
            _deck = data.Deck;
            Hand = data.Hand;
            Seed = data.Seed;
            
            Team = team;
            Id = id;
        }

        
        public void Heal(int amount)
        {
            if (amount <= 0)
            {
                Debug.LogError("[Player] Error: Heal amount cannot be <= 0");
                return;
            }

            Health = Math.Min(MaxHealth, Health + amount);
        }

        public void GainMana(int amount)
        {
            if (amount <= 0)
            {
                Debug.LogError("[Player] Error: Mana amount cannot be <= 0");
                return;
            }

            Mana += amount;
        }
        
        public void SpendMana(int amount)
        {
            if (amount <= 0)
            {
                Debug.LogError("[Player] Error: Mana amount cannot be <= 0");
                return;
            }

            Mana -= amount;
        }
        
        public void PerformDamage(int amount)
        {
            if (amount <= 0)
            {
                Debug.LogError("[Player] Error: Damage amount cannot be <= 0");
                return;
            }

            Health = Math.Max(0, Health - amount);
            // View?.UpdateData(new PlayerDataView()
            // {
            //     Health = Health,
            //     MaxHealth = MaxHealth,
            //     Mana = Mana,
            //     MaxMana = MaxHealth,
            //     Name = Name,
            //     GroundEnabled = false
            // });
            if (Health <= 0)
            {
                IsDead = true;
            }
        }
        
        void IPlayerData.UpdateDeck(ICardDeck deck) => _deck = deck;

        
        public void UpdateHand(ICardDeck hand)
        {
            if ((hand != null) && (hand.Cards != null))
                Hand = hand;
        }
        
        public abstract void StartTurn();
        public abstract void SetupMatchForTurn(IBoard battle);
    }
}