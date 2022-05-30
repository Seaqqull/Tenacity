using UnityEngine.Events;
using UnityEngine;
using System;


namespace Tenacity.General.MiniGames
{
    public abstract class MiniGame : MonoBehaviour
    {
        [Serializable]
        public class BoolEvent : UnityEvent<bool> { }

        
        [Header("Events")]
        [SerializeField] protected BoolEvent _onFinish;

        public bool Solved { get; private set; }

        protected Action<bool> _onClose;
        protected bool _closed;


        protected void OnDestroy()
        {
            if (!_closed)
                Close(false);
        }

        
        protected void Close(bool destroyItself = true)
        {
            _closed = true;
            
            _onClose?.Invoke(Solved);
            if (_onFinish != null)
                _onFinish.Invoke(Solved);
            if (destroyItself)
                Destroy(gameObject);
            
            _onClose = null;
        }

        protected abstract void Validate();

        
        public void Fail()
        {
            Solved = false;
            Close();
        }
        
        public void Succeed()
        {
            Solved = true;
            Close();
        }

        public void Show(Action<bool> finishCallback)
        {
            _onClose = finishCallback;
        }
    }
}