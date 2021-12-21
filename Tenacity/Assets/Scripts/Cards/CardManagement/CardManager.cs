using System;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField]
    private CardFactory _cardFactory;
    //private Card card;

    public CardFactory CardFactory { get => _cardFactory; }
    public Component[] CardComponents { get; private set; }

    public static string PropertyFactoryName = nameof(_cardFactory);

    private void Awake()
    {
       // card = _cardFactory.InitDefaultCard();
    }
}
