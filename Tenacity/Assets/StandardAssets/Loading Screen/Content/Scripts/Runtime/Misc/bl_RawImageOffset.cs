using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lovatto.SceneLoader
{
    [RequireComponent(typeof(RawImage))]
    public class bl_RawImageOffset : MonoBehaviour
    {
        public Vector2 OffsetScale = new Vector2(0, 1);
        private RawImage rawImage;

        private Rect offset;

        private void Awake()
        {
            rawImage = GetComponent<RawImage>();
        }

        private void Update()
        {
            offset = rawImage.uvRect;
            offset.position += OffsetScale;
            rawImage.uvRect = offset;
        }
    }
}