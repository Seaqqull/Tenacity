using UnityEngine;


namespace Tenacity.Managers
{
    public class StorageManager : Base.SingleBehaviour<StorageManager>
    {
        public Camera Camera { get; private set; }

        
        public void UpdateGameObjects()
        {
            Camera = Camera.main;
        }
    }
}