using UnityScenes = UnityEngine.SceneManagement;
using Tenacity.General.Interactions;
using System.Collections.Generic;
using Tenacity.General.Loading;
using Tenacity.Utility.Data;
using UnityEngine.Events;
using System.Collections;
using Tenacity.Dialogs;
using UnityEngine;
using System;


namespace Tenacity.Managers
{
    public class SceneManager : Base.SingleBehaviour<SceneManager>
    {
        #region Constants
        private const float HIDE_DIALOG_DELAY = 0.1f;
        private const int TEMPORARY_SCENE_INDEX = 2;
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

        private List<Dialog> _activeDialogs = new ();
        private Coroutine _hideDialogDelayCoroutine;
        private Coroutine _blockMouseClickCoroutine;
        private Coroutine _hideMouseDelayCoroutine;
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
        public int LevelIndex { get; private set; } = -1;
        public string LevelName { get; private set; }
        public bool MouseHoverVisible { get; set; } = true;
        public bool MouseClickBlocked { get; set; }
        public bool MouseActionAllowed
        {
            get => (_activeDialogs.Count == 0) && (_hideDialogDelayCoroutine == null) && !MouseClickBlocked;
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
        
        private IEnumerator UnblockMouseRoutine(float period = -1.0f, bool blockMouse = false)
        {
            if (blockMouse)
                MouseClickBlocked = true;
            
            yield return new WaitForSeconds((period <= 0.0f) ? HIDE_DIALOG_DELAY : period);
            
            _hideMouseDelayCoroutine = null;
            MouseClickBlocked = false;
        }

        private IEnumerator MouseHoverCoroutine()
        {
            var repeatPeriod = new WaitForSeconds(_mouseHoverPeriod);

            while (true)
            {
                var mouseHitInfo = RaycastManager.Instance.GetMovementPoint();
                
                if (!MouseHoverVisible && _mouseHover.activeSelf)
                    _mouseHover.SetActive(false);
                else if (MouseHoverVisible && (mouseHitInfo.HitSomePosition != _mouseHover.activeSelf))
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

        private IEnumerator LoadScene(int sceneToUnload, int sceneToLoad, string loadSceneName = "")
        {
            MouseClickBlocked = true;
            // Load loading scene
            var asyncLoad = UnityScenes.SceneManager.LoadSceneAsync(TEMPORARY_SCENE_INDEX, UnityScenes.LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
                yield return null;
            
            var loader = FindObjectOfType<SceneLoader>();
            loader.SceneName = loadSceneName;
            loader.OnLoad += () =>
            {
                UnityScenes.SceneManager.UnloadScene(TEMPORARY_SCENE_INDEX);
                _onLoadScene?.Invoke();
                
                MouseClickBlocked = false;
            };
            yield return null;
            
            if (sceneToUnload != -1)
                UnityScenes.SceneManager.UnloadScene(sceneToUnload);
            
            loader.Load(sceneToLoad);
        }
        
        
        public void HideMouseClick()
        {
            _mouseClick.SetActive(false);
        }

        public void UpdateLevelIndex(int index)
        {
            LevelIndex = index;
        }

        public void SetClickPosition(Vector3 position)
        {
            var mouseHitInfo = RaycastManager.Instance.GetMovementPoint(position);
            
            _mouseClick.transform.rotation = Quaternion.FromToRotation(Vector3.up, mouseHitInfo.HitData.Normal);
            _mouseClick.transform.position = mouseHitInfo.HitData.Position + (mouseHitInfo.HitData.Normal * _mouseUpShiftPositioning);
        }
        
        public void LoadLevel(int newLevelIndex, string screenName = "")
        {
            StartCoroutine(LoadScene(LevelIndex, newLevelIndex, screenName));

            LevelIndex = newLevelIndex;
            LevelName = screenName;
        }

        public void UnblockMouseWithDelay(float period = -1.0f, bool blockMouse = false)
        {
            _hideMouseDelayCoroutine = StartCoroutine(UnblockMouseRoutine(period, blockMouse));
        }
        

        public Dialog GetLastDialog()
        {
            return (_activeDialogs.Count == 0) ? null : _activeDialogs[^1];
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