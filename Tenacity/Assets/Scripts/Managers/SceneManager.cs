using System;
using System.Collections;
using Tenacity.Input.Data;
using Tenacity.Utility.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityScenes = UnityEngine.SceneManagement;


namespace Tenacity.Managers
{
    public class SceneManager : Base.SingleBehaviour<SceneManager>
    {
        [Serializable]
        private class VectorEvent : UnityEvent<MouseHitInfo> { }


        [Header("Mouse")] 
        [SerializeField] private float _mouseHoverPeriod;
        [SerializeField] private float _mouseUpShiftPositioning;
        [SerializeField] private GameObject _mouseHover;
        [SerializeField] private GameObject _mouseClick;
        [Header("Events")] 
        [SerializeField] private VectorEvent _onMouseMove;
        [SerializeField] private VectorEvent _onMouseClick;

        private Coroutine _mouseHoverRoutine;
        
        public event UnityAction<MouseHitInfo> MouseClick
        {
            add { _onMouseClick.AddListener(value); }
            remove { _onMouseClick.RemoveListener(value); }
        }
        public event UnityAction<MouseHitInfo> MouseMove
        {
            add { _onMouseMove.AddListener(value); }
            remove { _onMouseMove.RemoveListener(value); }
        }


        private void Start()
        {
            _mouseHoverRoutine = StartCoroutine(MouseHoverCoroutine());
            InputManager.MouseLeftButtonAction += OnMouseClick;
        }

        private void OnMouseClick(bool leftMouseButtonClicked)
        {
            if (leftMouseButtonClicked)
                return;
            var mouseHitInfo = RaycastManager.Instance.GetMovementPoint();
            if (!mouseHitInfo.HitSomePosition)
                return;
            
            _mouseClick.SetActive(true);
            _mouseClick.transform.rotation = Quaternion.FromToRotation(Vector3.up, mouseHitInfo.HitData.Normal);
            _mouseClick.transform.position = mouseHitInfo.HitData.Position + (mouseHitInfo.HitData.Normal * _mouseUpShiftPositioning);
            _onMouseClick?.Invoke(mouseHitInfo.HitData);
        }


        private IEnumerator MouseHoverCoroutine()
        {
            var repeatPeriod = new WaitForSeconds(_mouseHoverPeriod);

            while (true)
            {
                var mouseHitInfo = RaycastManager.Instance.GetMovementPoint();
                
                if (mouseHitInfo.HitSomePosition != _mouseHover.activeSelf)
                    _mouseHover.SetActive(mouseHitInfo.HitSomePosition);
                if (mouseHitInfo.HitSomePosition)
                {
                    _mouseHover.transform.rotation = Quaternion.FromToRotation(Vector3.up, mouseHitInfo.HitData.Normal);
                    _mouseHover.transform.position = mouseHitInfo.HitData.Position + (mouseHitInfo.HitData.Normal * _mouseUpShiftPositioning);
                    _onMouseMove?.Invoke(mouseHitInfo.HitData);
                }
                
                yield return repeatPeriod;
            }
        }

        
        public void LoadMainMenu()
        {
            UnityScenes.SceneManager.LoadScene(0);
        }

        public void LoadMainGame()
        {
            UnityScenes.SceneManager.LoadScene(1);
        }
        
        public void HideMouseClick()
        {
            _mouseClick.SetActive(false);
        }

        public void SetClickPosition(Vector3 position)
        {
            var mouseHitInfo = RaycastManager.Instance.GetMovementPoint(position);
            
            _mouseClick.transform.rotation = Quaternion.FromToRotation(Vector3.up, mouseHitInfo.HitData.Normal);
            _mouseClick.transform.position = mouseHitInfo.HitData.Position + (mouseHitInfo.HitData.Normal * _mouseUpShiftPositioning);
        }
    }
}