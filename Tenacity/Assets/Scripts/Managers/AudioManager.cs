using UnityEngine.Audio;
using UnityEngine;


namespace Tenacity.Managers
{
    public class AudioManager : Base.SingleBehaviour<AudioManager>
    {
        [SerializeField] private AudioMixerGroup _music;
        [SerializeField] private AudioMixerGroup _effects;

        public float Effects 
        {
            get
            {
                return PlayerPrefsManager.Instance.GetFloat(Utility.Constants.Audio.EFFECTS_COLUME,
                    Utility.Constants.Audio.MAX_LOUDNESS);
            }
        }
        public float Music 
        {
            get
            {
                return PlayerPrefsManager.Instance.GetFloat(Utility.Constants.Audio.MUSIC_COLUME,
                    Utility.Constants.Audio.MAX_LOUDNESS);
            }
        }


        private void Start()
        {
            var effectsVolume = PlayerPrefsManager.Instance.GetFloat(Utility.Constants.Audio.EFFECTS_COLUME,
                Utility.Constants.Audio.MAX_LOUDNESS);
            var musicVolume = PlayerPrefsManager.Instance.GetFloat(Utility.Constants.Audio.MUSIC_COLUME,
                Utility.Constants.Audio.MAX_LOUDNESS);

            _effects.audioMixer.SetFloat(Utility.Constants.Audio.EFFECTS_COLUME, effectsVolume);
            _music.audioMixer.SetFloat(Utility.Constants.Audio.MUSIC_COLUME, musicVolume);
        }
        

        public void UpdateMusicVolume(float loudness)
        {
            PlayerPrefsManager.Instance.SetFloat(Utility.Constants.Audio.MUSIC_COLUME, loudness);
            _music.audioMixer.SetFloat(Utility.Constants.Audio.MUSIC_COLUME, loudness);
        }
        
        public void UpdateEffectsVolume(float loudness)
        {
            PlayerPrefsManager.Instance.SetFloat(Utility.Constants.Audio.EFFECTS_COLUME, loudness);
            _effects.audioMixer.SetFloat(Utility.Constants.Audio.EFFECTS_COLUME, loudness);
        }
    }
}