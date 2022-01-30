using UnityEngine;


namespace Tenacity.Animation
{
    /// <summary>
    /// Updated direction of the GameObject using LocalScale and Animator property
    /// </summary>
    public class AnimationDirection : Base.BaseMono
    {
        [SerializeField] private bool _reverse;

        private Animator _controller;


        protected override void Awake()
        {
            base.Awake();

            _controller = GetComponent<Animator>();
        }

        private void Update()
        {
            int direction = _controller.GetInteger(Utility.Constants.Animation.DIRECTION);

            if(direction != 0)
                Transform.localScale = new Vector3(((_reverse) ? (-direction) : direction), 1, 1);
        }
    }
}
