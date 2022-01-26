


namespace Tenacity.Behaviour
{
    public class Player : Entity
    {
        public static Player Instance { get; private set; }


        protected override void Awake()
        {
            base.Awake();

            Instance = this;
        }
    }
}