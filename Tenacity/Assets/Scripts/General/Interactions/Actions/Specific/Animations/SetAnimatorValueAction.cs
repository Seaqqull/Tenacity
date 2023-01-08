using UnityEngine;


namespace Tenacity.General.Interactions.Actions.Animations
{
    public enum AnimatorParameterType { Float, Int, Bool, Trigger }
    [System.Serializable]
    public class AnimatorParameter
    {
        public string Name;
        public AnimatorParameterType Type;
        [Header("Values")]
        public float Float;
        public int Int;
        public bool Bool;
    }

    [CreateAssetMenu(fileName = "AnimatorAction", menuName = "Action/Interaction/AnimatorAction", order = 0)]
    public class SetAnimatorValueAction : InteractionAction
    {
        [SerializeField] private AnimatorParameter[] _parameters;
        
        
        public override void Execute(Interaction interaction, Collider intruder)
        {
            var animator = interaction.GetComponentInChildren<Animator>();
            if (animator == null) return;

            foreach (var parameter in _parameters)
            {
                switch (parameter.Type)
                {
                    case AnimatorParameterType.Float:
                        animator.SetFloat(parameter.Name, parameter.Float);
                        break;
                    case AnimatorParameterType.Int:
                        animator.SetInteger(parameter.Name, parameter.Int);
                        break;
                    case AnimatorParameterType.Bool:
                        animator.SetBool(parameter.Name, parameter.Bool);
                        break;
                    case AnimatorParameterType.Trigger:
                        animator.SetTrigger(parameter.Name);
                        break;
                    default:
                        return;
                }
            }
        }
    }
}