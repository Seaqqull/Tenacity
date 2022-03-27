using UnityEngine;


namespace Tenacity.General
{
    public class ObjectSaver : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}