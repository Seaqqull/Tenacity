using UnityEngine;


namespace Tenacity.Utility
{
    public class ObjectSaver : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}