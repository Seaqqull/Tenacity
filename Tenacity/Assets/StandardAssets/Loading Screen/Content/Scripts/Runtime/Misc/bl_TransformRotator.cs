using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lovatto.SceneLoader
{
    public class bl_TransformRotator : MonoBehaviour
    {
        public float Speed = 10;
        public RectTransform rectTransform;

        private void Update()
        {
            rectTransform.Rotate(Vector3.forward * Time.deltaTime * Speed);
        }
    }
}