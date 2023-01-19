using System.Collections.Generic;
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
        public static readonly int IS_ACTIVE = Animator.StringToHash("IsActive");
        public static readonly int RUNNING = Animator.StringToHash("IsRunning");
        public static readonly int WALKING = Animator.StringToHash("IsWalking");
        public static readonly int FALLING = Animator.StringToHash("IsFalling");
        public static readonly int ATTACK = Animator.StringToHash("IsAttack");
        public static readonly int MOVE = Animator.StringToHash("InMove");
        public static readonly int JUMP = Animator.StringToHash("IsJump");
        public static readonly int SIDE = Animator.StringToHash("Side");
        public static readonly int DEAD = Animator.StringToHash("Die");
        public static readonly int HIT = Animator.StringToHash("Hit");
        public static readonly int OFF = Animator.StringToHash("Off");
        public static readonly int ON = Animator.StringToHash("On");
    }

    public static class Game
    {
        public const string TIME_SCALE = "TimeScale";
        public const string FRAMERATE = "Framerate";
        
        public const float TIME_SCALE_MIN = 1.0f;
        public const float TIME_SCALE_MAX = 100;
    }

    public static class Audio
    {
        public const string EFFECTS_COLUME = "EffectsVolume";
        public const string MUSIC_COLUME = "MusicVolume";

        public const float MIN_LOUDNESS = -80;
        public const float MAX_LOUDNESS = 20;

    }

    public static class PlayerPrefs
    {
        public const string LOCALE = "language";
    }

    public static class Scenes
    {
        public const int MAIN_MENU = 0;
        public const int MAIN_GAME = 0;
    }

    public static class Framerate
    {
        public static List<int> FRAME_RATES = new() { 30, 60, 120 };
        public const int DEFAULT_REFRESH_RATE = 60;

        private static List<int> _allowed_framerate;

        public static List<int> ALLOWED_FRAMERATE
        {
            get => _allowed_framerate ??= IsDefaultRefreshRate ? FRAME_RATES.GetRange(0, 2) : FRAME_RATES;
        }


        public static bool IsDefaultRefreshRate
        {
            get => Screen.currentResolution.refreshRate <= DEFAULT_REFRESH_RATE;
        }

        public static int FramerateFromIndex(int index)
        {
            return ALLOWED_FRAMERATE[index];
        }
    }

    public static class Debug
    {
        public const string DIALOG_SYSTEM = "Dialog";
        public const string EVENT_SYSTEM = "Event";
    }
}