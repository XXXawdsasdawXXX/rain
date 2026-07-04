
using Cysharp.Threading.Tasks;
using FMOD.Studio;
using FMODUnity;
using UnityEngine.Scripting;

namespace Core.Audio.AudioSystem
{
    [Preserve]
    public class AudioGlobalVolume 
    {
        private const string MUSIC_VCA_PATH_VCA = "vca:/Music";
        private const string SFX_VCA_PATH = "vca:/SFX";
        
        public bool IsInitialized { get; set; }

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
        }

        public void SetSFXVolume(float volume)
        {
            _sfxVCA.setVolume(volume);

        }
    }
}