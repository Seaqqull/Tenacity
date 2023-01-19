using Tenacity.Managers;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;


namespace Tenacity.UI.Menus.UI.Menus
{
    public struct EndBattleInfo
    {
        public string InfoText;
        public Action OnClose;
        public bool IsWin;
    }
    
    
    public class EndBattleMenu : SingleMenu<EndBattleMenu>
    {
        [SerializeField] private Color _winColor;
        [SerializeField] private Color _loseColor;
        [Space] 
        [SerializeField] private GameObject _infoPanel;
        [SerializeField] private Image _battleResultImage;
        [SerializeField] private TMP_Text _infoText;

        private Action _onClose;


        public override void OnBackAction()
        {
            MenuManager.Instance.CloseMenu(this);
            InputQueueManager.Instance.OnHideInGameMenu();
            
            _onClose?.Invoke();

            _battleResultImage.color = Color.white;
            _infoText.text = string.Empty;
            _onClose = null;
        }
        
        public void ShowBattleResult(EndBattleInfo info)
        {
            _battleResultImage.color = (info.IsWin) ? _winColor : _loseColor;
            _infoText.text = info.InfoText;
            _onClose = info.OnClose;
            
            _infoPanel.SetActive(true);
        }
    }
}