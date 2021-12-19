using UnityEngine;


namespace Tenacity.Utility.Base
{
    public class BaseMono : MonoBehaviour
    {
        public GameObject GameObject { get; private set; }
        public Transform Transform { get; private set; }

        
        protected virtual void Awake()
        {
            GameObject = gameObject;
            Transform = transform;
        }
    }
}
