using UnityEngine;


namespace Tenacity.Behaviour.Enemies.AniamtionEvents
{
    /// <summary>
    /// Base class for handling Enemy animation events
    /// </summary>
    public abstract class EnemyAnimationEvent : AnimationEvent
    {
        [SerializeField] protected Behaviour.Enemies.Enemy _enemy;
    }
}
