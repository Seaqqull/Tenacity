using UnityEngine;


namespace Tenacity.General.Interactions.Actions
{
    [CreateAssetMenu(menuName = "Action/Interaction/NPCAction")]
    public class NPCAction : ShowDialogAction
    {
        private Animator _animator;
        
        
        private void Initialize(Interaction interaction)
        {
            _animator = interaction.GetComponentInChildren<Animator>();
        }

        
        public override void OnEnter(Interaction interaction, Collider intruder)
        {
            if(_animator == null)
                _animator = interaction.GetComponentInChildren<Animator>();
            _animator.SetBool(Tenacity.Utility.Constants.Animation.IS_ACTIVE, true);
        }

        public override void OnExit(Interaction interaction, Collider intruder)
        {
            if(_animator == null)
                _animator = interaction.GetComponentInChildren<Animator>();
            _animator.SetBool(Tenacity.Utility.Constants.Animation.IS_ACTIVE, false);
            _animator = null;
        }


        public override void Execute(Interaction interaction, Collider intruder)
        {
            base.Execute(interaction, intruder);
            
            if(_animator == null)
                _animator = interaction.GetComponentInChildren<Animator>();
            _animator.SetBool(Tenacity.Utility.Constants.Animation.IS_ACTIVE, false);
        }
        
        
        public void ShowGreeting()
        {
            if(_animator != null)
                _animator.SetBool(Tenacity.Utility.Constants.Animation.IS_ACTIVE, true);
        }
    }
}