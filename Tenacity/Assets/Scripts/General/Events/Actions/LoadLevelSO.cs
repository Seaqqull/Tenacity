using Tenacity.General.SaveLoad.Implementation;
using Tenacity.Properties;
using Tenacity.Managers;
using UnityEngine;


namespace Tenacity.General.Events.Actions
{
    [CreateAssetMenu(menuName = "Events/Actions/LoadLevel", fileName = "LoadLevel", order = 0)]
    public class LoadLevelSO : ActionSO<bool>
    {
        [SerializeField] private string _sceneName;
        [SerializeField] private bool _clearGame;
        
        
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
            if (_clearGame)
                World.Instance.Clear();

            SceneManager.Instance.LoadLevel(levelId.Value, _sceneName, () =>
            {
                if (_clearGame)
                    PlayerManager.Instance.gameObject.GetComponent<Player.Player>().ResetInventory();
            });
        }
    }
}