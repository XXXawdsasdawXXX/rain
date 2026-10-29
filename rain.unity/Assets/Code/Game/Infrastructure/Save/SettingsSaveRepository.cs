using System;
using Core.Save;

namespace Code.Game.Save
{
    public sealed class SettingsSaveRepository : ASaveRepository<SettingsSaveRepository.Model>
    {
        [Serializable]
        public sealed class Model
        {
            public float MusicVolume;
            public float SfxVolume;
        }
        
        public SettingsSaveRepository(ISaveProcessor processor) : base(processor)
        {
        }

        protected override Model createDefault()
        {
            return new Model
            {
                MusicVolume = 0.7f,
                SfxVolume = 0.7f
            };
        }
    }
}