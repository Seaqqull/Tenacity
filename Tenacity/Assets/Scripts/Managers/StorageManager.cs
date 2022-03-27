using UnityEngine;


namespace Tenacity.Managers
{
    public class StorageManager : Base.SingleBehaviour<StorageManager>
    {
        #region Constants
        private const string CAMERA_UI = "UICamera";
        #endregion
        
        public Camera MainCamera { get; private set; }
        public Camera UICamera { get; private set; }

        
        public void UpdateGameObjects()
        {
            MainCamera = Camera.main;
            UICamera = GameObject.FindGameObjectWithTag(CAMERA_UI).GetComponent<Camera>();
        }
    }
}