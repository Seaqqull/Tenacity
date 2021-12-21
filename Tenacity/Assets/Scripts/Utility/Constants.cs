using UnityEngine;


namespace Tenacity.Utility.Constants
{
    /// <summary>
    /// Animation hashed ids
    /// </summary>
    public static class Animation
    {
        public static readonly int MOVE_SPEED = Animator.StringToHash("MovementSpeed");
        public static readonly int DIRECTION = Animator.StringToHash("Direction");
        public static readonly int FALLING = Animator.StringToHash("IsFalling");
        public static readonly int ATTACK = Animator.StringToHash("IsAttack");
        public static readonly int MOVE = Animator.StringToHash("InMove");
        public static readonly int JUMP = Animator.StringToHash("IsJump");
        public static readonly int DEAD = Animator.StringToHash("Die");
        public static readonly int HIT = Animator.StringToHash("Hit");
    }

    public static class Audio
    {
        public const string EFFECTS_COLUME = "EffectsVolume";
        public const string MUSIC_COLUME = "MusicVolume";

        public const float MIN_LOUDNESS = -80;
        public const float MAX_LOUDNESS = 0;

    }

    public static class PlayerPrefs
    {
        public const string LOCALE = "language";
    }
}