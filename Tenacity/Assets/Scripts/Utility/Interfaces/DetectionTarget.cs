using UnityEngine;


namespace Tenacity.Utility
{
    [System.Serializable]
    public class DetectionTarget
    {
        public Vector3 Direction { get; set; }
        public float RelativeDistance { get; set; }
    }
}
