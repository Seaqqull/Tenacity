using System.Collections;
using Tenacity.Cards;
using UnityEngine;

namespace Assets.Scripts.Battles.Enemies
{
    public class BattleEnemy : MonoBehaviour
    {
        [SerializeField] private BattleCharacterSO _enemy;
        [SerializeField] private Vector3 _enemyPos;
        [SerializeField] private BattleCardDeckManager _enemyCardDeck;


    }
}