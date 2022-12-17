using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;


namespace Tenacity.General.Loading
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private TMPro.TMP_Text _sceneNameText;
        [SerializeField] private string _loadingTextMask;
        [SerializeField] private TMPro.TMP_Text _progressText;
        [SerializeField] private Image _filledImage;
        [SerializeField] private GameObject _skipKeyText;
        [SerializeField] private  GameObject _continueUI;

        private Action _onLoad;

        public string SceneName
        {
            get => _sceneNameText.text;
            set => _sceneNameText.text = value;
        }
        public event Action OnLoad
        {
            add { _onLoad += value; }
            remove { _onLoad -= value; }
        }

        
        private void Awake()
        {
            Init();
        }

        
        private void Init()
        {
            if (_skipKeyText != null)
                _skipKeyText.SetActive(false);
            if (_filledImage != null)
            {
                _filledImage.type = Image.Type.Filled;
                _filledImage.fillAmount = 0;
            }
            if (_continueUI != null)
                _continueUI.SetActive(false);
        }

        private void Finish()
        {
            if (_skipKeyText != null)
                _skipKeyText.SetActive(true);
            if (_continueUI != null)
                _continueUI.SetActive(true);
        }

        private IEnumerator LoadSceneRoutine(int sceneIndex)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
            {
                UpdateLoadProgress(asyncLoad.progress, asyncLoad.progress - 0.1f);
                yield return null;
            }
            
            yield return null;
            UpdateLoadProgress(1.0f, 1.0f);
            Finish();
        }

        private void UpdateLoadProgress(float value, float delayedValue)
        { 

            if (_filledImage != null)
                _filledImage.fillAmount = value;
            if (_progressText != null )
            {
                string percent = (delayedValue * 100).ToString("F0");
                _progressText.text = string.Format(_loadingTextMask, percent);;
            }
        }
        
        
        public void Load(int sceneIndex)
        {
            StartCoroutine(LoadSceneRoutine(sceneIndex));
        }

        public void CloseLoadScene()
        {
            _onLoad?.Invoke();
        }
    }
}
