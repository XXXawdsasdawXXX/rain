using Code.Game.Save;
using Code.UI.Components;
using Code.UI.Components.Window;
using UnityEngine;

namespace Code.UI.Menu
{
    public sealed class SettingsWindowView : AUIWindowView
    {
        [field: SerializeField] public UISlider Slider_SFX { get; private set; }
        [field: SerializeField] public UISlider Slider_Music { get; private set; }
    }
    
    public sealed class SettingsWindowController : AUIWindowController<SettingsWindowView>
    {
        private readonly SettingsSaveRepository _settingsSave;

        
        public SettingsWindowController(SettingsWindowView view, SettingsSaveRepository settingsSave) : base(view)
        {
            _settingsSave = settingsSave;
        }

        protected override void subscribe()
        {
            view.Slider_Music.Changed += _onChangedMusicVolume;
            view.Slider_SFX.Changed += _onChangedSFXVolume;
        }
        
        protected override void unSubscribe()
        {
            view.Slider_Music.Changed -= _onChangedMusicVolume;
            view.Slider_SFX.Changed -= _onChangedSFXVolume;
        }
        
        private void _onChangedSFXVolume(float value)
        {
            _settingsSave.Data.SfxVolume = value;
        }

        private void _onChangedMusicVolume(float value)
        {
            _settingsSave.Data.MusicVolume = value;
        }
    }
}