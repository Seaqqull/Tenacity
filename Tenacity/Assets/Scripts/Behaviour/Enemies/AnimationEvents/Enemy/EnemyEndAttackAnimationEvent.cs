


namespace Tenacity.Behaviour.Enemies.AniamtionEvents
{
    /// <summary>
    /// Handler for Enemy EndAttack event
    /// </summary>
    public class EnemyEndAttackAnimationEvent : EnemyAnimationEvent
    {
        public void OnEndAttackEvent()
        {
            _enemy.OnEndAttack();
        }
    }
}
