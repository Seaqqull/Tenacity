using Tenacity.Properties;
using UnityEngine.Events;
using Tenacity.Managers;
using UnityEngine;


namespace Tenacity.General.Events.Actions
{
    [CreateAssetMenu(menuName = "Events/Actions/LoadLevelAfterMiniGame", fileName = "LoadLevelAfterMiniGame", order = 0)]
    public class LoadLevelAfterMiniGameSO : ActionSO<bool>
    {
        [SerializeField] private IntegerVariable _miniGameId;
        [SerializeField] private string _sceneName;
        [Space] 
        [SerializeField] private UnityEvent _onPerform;
        [SerializeField] private UnityEvent _onClose;
        
        
        protected override bool PerformAction()
        {
            return true;
        }

        public override bool Perform()
        {
            return true;
        }
        
        public void Perform(IntegerVariable levelId)
        {
            var levelIndex = levelId.Value;
            _onPerform.Invoke();
            
            MiniGameManager.Instance.Show(_miniGameId.Value, (result) => {
                _onClose.Invoke();
                if (result)
                    SceneManager.Instance.LoadMainGame(levelIndex, _sceneName);
            });
        }
    }
}