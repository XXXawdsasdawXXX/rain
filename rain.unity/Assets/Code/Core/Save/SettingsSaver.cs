using System;
using Core.Save.Models;

namespace Core.Save
{
    public class SettingsSaver : Saver<SettingsSaver.Model>
    {
        [Serializable]
        public class Model : SavedModel
        {
            public float SFXVolume;
            public float MusicVolume;

            public Model()
            {
                SFXVolume = 0.7f;
                MusicVolume = 0.7f;
            }
        }
        
        protected override Model getNewModel()
        {
            return new Model();
        }
    }
}