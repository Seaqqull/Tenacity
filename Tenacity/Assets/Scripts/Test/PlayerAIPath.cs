using Tenacity.Managers;
using Pathfinding;
using UnityEngine;


namespace Tenacity.Test
{
    public class PlayerAIPath : AIPath
    {
        private bool _reachedPath;
        
        
        protected override void Start()
        {
            base.Awake();
            
            SceneManager.Instance.MouseClick += OnMouseClick;
            //this.;
        }
        
        protected override void Update()
        {
            base.Update();
            
            if (_reachedPath || !reachedDestination)
                return;
            
            _reachedPath = true;
            SceneManager.Instance.HideMouseClick();
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            
            SceneManager.Instance.MouseClick -= OnMouseClick;
        }

        protected override void OnPathComplete(Path newPath)
        {
            base.OnPathComplete(newPath);
            
            SceneManager.Instance.SetClickPosition(newPath.vectorPath[newPath.vectorPath.Count - 1]);
        }

        private void OnMouseClick(Utility.Data.MouseHitInfo mouseInfo)
        {
            // this.pathPending
            // this.onSearchPath
            destination = mouseInfo.Position;
            _reachedPath = false;
        }
    }
}