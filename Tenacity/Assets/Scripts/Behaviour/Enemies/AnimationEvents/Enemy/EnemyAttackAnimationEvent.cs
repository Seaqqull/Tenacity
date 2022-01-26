using UnityEngine;


namespace Tenacity.Behaviour.Enemies.AniamtionEvents
{
    /// <summary>
    /// Handler for Enemy Attack event
    /// </summary>
    public class EnemyAttackAnimationEvent : EnemyAnimationEvent
    {
        public void OnAttackEvent()
        {
            _enemy.OnAttack();
        }
    }
}
