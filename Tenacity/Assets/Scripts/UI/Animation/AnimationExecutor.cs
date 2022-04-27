using UnityEngine;


namespace Tenacity.UI.Animation
{
    [RequireComponent(typeof(Animator))]
    public class AnimationExecutor : MonoBehaviour
    {
        [SerializeField] private Animator _animation;


        private void Awake()
        {
            if (_animation == null)
                _animation = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _animation.SetTrigger(Utility.Constants.Animation.ON);
        }

        private void OnDisable()
        {
            _animation.SetTrigger(Utility.Constants.Animation.OFF);
        }
    }
}