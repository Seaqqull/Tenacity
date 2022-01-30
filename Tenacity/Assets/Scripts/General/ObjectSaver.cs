using UnityEngine;


namespace Tenacity.Genral
{
    public class ObjectSaver : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}