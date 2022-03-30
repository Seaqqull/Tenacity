using UnityEngine;
using System;

namespace Lovatto.SceneLoader
{
    [Serializable]
    public class bl_SceneLoaderInfo
    {
        [Header("Scene Info")]
        [HideInInspector] public string SceneName = "Scene Name";
#if UNITY_EDITOR
        [SerializeField]
        public UnityEngine.Object SceneAsset;
#endif
        public string DisplayName = "Display Name";
        [TextArea(3,7)]public string Description = "";

        [Header("Settings")]
        public SceneSkipType SkipType = SceneSkipType.InstantComplete;
        public LoadingType LoadingType = LoadingType.Async;
        [Range(0.1f, 30)] public float FakeLoadingTime = 2f;

        [Header("References")]
        public Sprite[] Backgrounds = null;
    }
}