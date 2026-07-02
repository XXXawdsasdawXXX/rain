using Core.GameLoop;
using Core.Save;
using Core.ServiceLocator;
using Core.ServiceLocator.MarkerInterfaces;
using Cysharp.Threading.Tasks;
using FMOD.Studio;
using FMODUnity;
using UnityEngine.Scripting;

namespace Core.Audio.AudioSystem
{
    [Preserve]
    public class AudioGlobalVolume : IService, IInitializeListener
    {
        private const string MUSIC_VCA_PATH_VCA = "vca:/Music";
        private const string SFX_VCA_PATH = "vca:/SFX";
        
        public bool IsInitialized { get; set; }
        public float MusicValue => _settingsSaver.Current.MusicVolume;
        public float SFXValue => _settingsSaver.Current.SFXVolume;
        
        private SettingsSaver _settingsSaver;
        
        private VCA _musicVCA;
        private VCA _sfxVCA;

        
        public UniTask Initialize()
        {
            _musicVCA = RuntimeManager.GetVCA(MUSIC_VCA_PATH_VCA);
            _sfxVCA = RuntimeManager.GetVCA(SFX_VCA_PATH);
                        
            return UniTask.CompletedTask;
        }
        
        public void SetMusicVolume(float volume)
        {
            _musicVCA.setVolume(volume);
            _settingsSaver.Current.MusicVolume = volume;
        }

        public void SetSFXVolume(float volume)
        {
            _sfxVCA.setVolume(volume);
            _settingsSaver.Current.SFXVolume = volume;
        }
    }
}