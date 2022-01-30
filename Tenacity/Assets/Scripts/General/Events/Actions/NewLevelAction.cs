using UnityEngine;


namespace Tenacity.General.Events.Actions
{
    public class NewLevelAction : MonoBehaviour
    {
        private void Start()
        {
            Managers.StorageManager.Instance.UpdateGameObjects();
        }
    }
}