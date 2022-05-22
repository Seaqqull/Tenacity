using System.Collections.Generic;
using Tenacity.General.MiniGames;
using Tenacity.Base;
using UnityEngine;
using System;


namespace Tenacity.Managers
{
    public class MiniGameManager : SingleBehaviour<MiniGameManager>
    {
        [SerializeField] private List<MiniGame> _games;
        
        private Transform _transform;
        private Action _onSuccess;
        private Action _onFail;
        
        public bool GameActive { get; private set; }


        protected override void Awake()
        {
            base.Awake();
            
            _transform = transform;
        }
        

        private void ShowGame(int gameIndex, Action<bool> finishCallback)
        {
            var miniGame = Instantiate(_games[gameIndex], _transform);
            miniGame.Show((completed) =>
            {
                GameActive = false;
                finishCallback?.Invoke(completed);
            });
        }

        
        public void ShowGame(int gameIndex)
        {
            ShowGame(gameIndex, result => {});
        }

        public void ShowRandom(Action<bool> finishCallback)
        {
            var miniGameToShow = UnityEngine.Random.Range(0, _games.Count);
            
            GameActive = true;
            ShowGame(miniGameToShow, finishCallback);
        }
        
        public void Show(int gameIndex, Action<bool> finishCallback)
        {
            if ((gameIndex < 0) || (gameIndex >= _games.Count))
                return;

            GameActive = true;
            ShowGame(gameIndex, finishCallback);
        }
    }
}
