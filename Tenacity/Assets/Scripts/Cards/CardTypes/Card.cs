using System;
using UnityEngine;

[Serializable]
public class Card
{
    private enum RarityTier
    {
        None = 0,
        Common = 1,
        Rare = 2,
        Epic = 4,
        Legendary = 8
    }

    [SerializeField]
    private int cardID;
    [SerializeField]
    private string cardName;
    [SerializeField, Multiline]
    private string cardDescription;
    [SerializeField]
    private int manaCost;
    [SerializeField]
    private RarityTier rarityTier;
    [SerializeField]
    private Sprite cardSprite;

}
