using System;
using System.Collections;
using System.Collections.Generic;
using Tenacity.Dialogs;
using Tenacity.General.Interactions;
using Tenacity.Utility.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityScenes = UnityEngine.SceneManagement;


namespace Tenacity.Managers
{
    public class SceneManager : Base.SingleBehaviour<SceneManager>
    {
        #region Constants
        private const float HIDE_DIALOG_DELAY = 0.1f;
        #endregion
        [Serializable]
        private class VectorEvent : UnityEvent<MouseHitInfo> { }


        [Header("Mouse")] 
        [SerializeField] private float _mouseHoverPeriod;
        [SerializeField] private float _mouseUpShiftPositioning;
        [SerializeField] private GameObject _mouseHover;
        [SerializeField] private GameObject _mouseClick;
        [Header("Events")] 
        [SerializeField] private UnityEvent _onLoadScene;
        [SerializeField] private VectorEvent _onMouseMove;
        [SerializeField] private VectorEvent _onMouseClick;

        private List<Dialog> _activeDialogs = new List<Dialog>();
        private Coroutine _hideDialogDelayCoroutine;
        private Coroutine _mouseHoverRoutine;
        private int _levelIndex = - 1;
        
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
        public bool MouseActionAllowed
        {
            get => (_activeDialogs.Count == 0) && (_hideDialogDelayCoroutine == null);
        }


        private void Start()
        {
            _mouseHoverRoutine = StartCoroutine(MouseHoverCoroutine());
            InputManager.MouseLeftButtonAction += OnMouseClick;
        }

        private void OnMouseClick(bool leftMouseButtonClicked)
        {
            if (leftMouseButtonClicked || !MouseActionAllowed)
                return;

            var interactionHitInfo = RaycastManager.Instance.GetInteractionPoint();
            if (interactionHitInfo.HitSomePosition)
            {
                var interactionObject = interactionHitInfo.HitData.Object.GetComponentInParent<Interaction>();
                if (interactionObject != null)
                {
                    interactionObject.Interact();
                    return;
                }
            }

            var movementHitInfo = RaycastManager.Instance.GetMovementPoint();
            if (!movementHitInfo.HitSomePosition)
                return;
            
            _mouseClick.SetActive(true);
            _mouseClick.transform.rotation = Quaternion.FromToRotation(Vector3.up, movementHitInfo.HitData.Normal);
            _mouseClick.transform.position = movementHitInfo.HitData.Position + (movementHitInfo.HitData.Normal * _mouseUpShiftPositioning);
            _onMouseClick?.Invoke(movementHitInfo.HitData);
        }

        private IEnumerator HideDialogRoutine()
        {
            yield return new WaitForSeconds(HIDE_DIALOG_DELAY);
            _hideDialogDelayCoroutine = null;
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

        private IEnumerator LoadScene(int sceneToUnload, int sceneToLoad)
        {
            AsyncOperation asyncLoad = UnityScenes.SceneManager.LoadSceneAsync(sceneToLoad, UnityScenes.LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
                yield return null;
            
            if (sceneToUnload != -1)
                UnityScenes.SceneManager.UnloadScene(sceneToUnload);
            
            yield return null;
            _onLoadScene?.Invoke();
        }


        public void LoadMainMenu()
        {
            StartCoroutine(LoadScene(_levelIndex, 0));
            _levelIndex = -1;
            // UnityScenes.SceneManager.LoadScene(0, UnityScenes.LoadSceneMode.Additive);
            // if (_levelIndex != -1)
            // {
            //     UnityScenes.SceneManager.UnloadScene(_levelIndex);
            //     _levelIndex = -1;
            // }
        }

        public void LoadMainGame(int levelIndex = 1)
        {
            StartCoroutine(LoadScene((_levelIndex == -1) ? 0 : _levelIndex, levelIndex));
            _levelIndex = -1;
            
            // UnityScenes.SceneManager.LoadScene(levelIndex, UnityScenes.LoadSceneMode.Additive);
            // if (_levelIndex != -1)
            // {
            //     UnityScenes.SceneManager.UnloadScene(_levelIndex);
            //     _levelIndex = -1;
            // }
            // else
            // {
            //     UnityScenes.SceneManager.UnloadScene(0);
            // }

            _levelIndex = levelIndex;
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
        

        public void AttachDialog(Dialog dialog)
        {
            if(!IsDialogAttached(dialog))
                _activeDialogs.Add(dialog);
        }

        public void DetachDialog(Dialog dialog)
        {
            if(IsDialogAttached(dialog))
            {
                _activeDialogs.Remove(dialog);
                if (_activeDialogs.Count == 0)
                    _hideDialogDelayCoroutine = StartCoroutine(HideDialogRoutine());
            }
        }
        
        public bool IsDialogAttached(Dialog dialog)
        {
            return _activeDialogs.Contains(dialog);
        }
    }
}