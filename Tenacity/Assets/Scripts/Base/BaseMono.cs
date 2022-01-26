using UnityEngine;


namespace Tenacity.Base
{
    public class BaseMono : MonoBehaviour
    {
        public GameObject GameObject { get; private set; }
        public Transform Transform { get; private set; }
        public Vector3 Position
        {
            get => Transform.position;
        }

        
        protected virtual void Awake()
        {
            GameObject = gameObject;
            Transform = transform;
        }
    }
}
